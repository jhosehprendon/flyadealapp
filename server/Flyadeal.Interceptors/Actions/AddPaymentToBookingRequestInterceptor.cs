using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using Microsoft.AspNetCore.Http.Features;
using System.Linq;
using System;
using Newskies.WebApi.Contracts.Enumerations;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Text;
using Flyadeal.Interceptors.Helpers;
using Newskies.VoucherManager;

namespace Flyadeal.Interceptors.Actions
{
    class AddPaymentToBookingRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var requestData = request as AddPaymentToBookingRequestData;
            if (requestData == null)
                return await Task.FromResult(requestData);
            var httpContext = context.HttpContext;
            var sessionBagService = httpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null || (await sessionBagService.Booking()) == null || string.IsNullOrEmpty(await sessionBagService.Signature()))
                return await Task.FromResult(request);
            var resourcesService = httpContext.RequestServices.GetService(typeof(IResourcesService)) as ResourcesService;
            if (resourcesService == null)
                return await Task.FromResult(request);
            var bookingService = httpContext.RequestServices.GetService(typeof(IBookingService)) as BookingService;
            if (bookingService == null)
                return await Task.FromResult(request);
            var paymentsService = httpContext.RequestServices.GetService(typeof(IPaymentsService)) as PaymentsService;
            if (paymentsService == null)
                return await Task.FromResult(request);
            var options = httpContext.RequestServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            if (options == null || options.Value == null || options.Value.PaymentSettings == null || options.Value.ApplicationSessionOptions == null)
                return await Task.FromResult(request);

            // populate payment method based on payment method code
            var paymentMethodsList = await resourcesService.GetPaymentMethodsList(await sessionBagService.CultureCode());
            if (paymentMethodsList == null || paymentMethodsList.PaymentMethodList == null)
                return await Task.FromResult(request);
            var selectedPayment = paymentMethodsList.PaymentMethodList.ToList().Find(
                p => p.PaymentMethodCode.Equals(requestData.PaymentMethodCode, StringComparison.OrdinalIgnoreCase));
            if (selectedPayment == null)
                return await Task.FromResult(request);
            requestData.PaymentMethodType = selectedPayment.PaymentMethodType == PaymentMethodType.ExternalAccount ? RequestPaymentMethodType.ExternalAccount
                : selectedPayment.PaymentMethodType == PaymentMethodType.PrePaid ? RequestPaymentMethodType.PrePaid
                : selectedPayment.PaymentMethodType == PaymentMethodType.AgencyAccount ? RequestPaymentMethodType.AgencyAccount
                : selectedPayment.PaymentMethodType == PaymentMethodType.Voucher ? RequestPaymentMethodType.Voucher
                : selectedPayment.PaymentMethodType == PaymentMethodType.Loyalty ? RequestPaymentMethodType.Loyalty
                : RequestPaymentMethodType.Unmapped;

            // populate currency and amount due
            var booking = await bookingService.GetSessionBooking(true);
            if (booking == null)
                return await Task.FromResult(request);
            requestData.QuotedCurrencyCode = booking.CurrencyCode;
            requestData.Installments = selectedPayment.MaxInstallments;

            // credit card payment
            if (Constants.CardPaymentMethodCodes.ToList().Contains(selectedPayment.PaymentMethodCode))
            {
                // for credit card payments, balance due is to be paid
                requestData.QuotedAmount = booking.BookingSum.BalanceDue; 

                // populate payment fields and 3DS request values
                var paymentFields = new List<PaymentField>
                {
                    new PaymentField
                    {
                        FieldName = "CC::AccountHolderName",
                        FieldValue = requestData.AccountHolderName
                    },
                    new PaymentField
                    {
                        FieldName = "CC::VerificationCode",
                        FieldValue = requestData.CVVCode
                    }
                };

                var httpConnectionFeature = httpContext.Features.Get<IHttpConnectionFeature>();
                var fwdedForHeader = context.HttpContext.Request.Headers["X-Forwarded-For"];
                var ipaddress = fwdedForHeader.Count() > 0 ? fwdedForHeader[0] : httpConnectionFeature?.RemoteIpAddress.ToString();
                ipaddress = !string.IsNullOrEmpty(ipaddress) ? ipaddress.Trim() : "";
                //ipaddress = ipaddress.Length > 20 ? ipaddress.Substring(0, 20) : ipaddress;
                ipaddress = ipaddress == "::1" ? "123.3.53.105" : ipaddress; // if localhost, put in some dummy IP address
                var acceptTypes = httpContext.Request.Headers["Accept"];
                var userAgent = httpContext.Request.Headers["User-Agent"];
                var redirectFrom3DSUrlTemplate = options.Value.PaymentSettings.RedirectFrom3DSUrlTemplate ?? "";
                if (!string.IsNullOrEmpty(redirectFrom3DSUrlTemplate))
                {
                    var sessionTokenHeader = options.Value.ApplicationSessionOptions.SessionTokenHeader;
                    var sessionTokenPresented = httpContext.Request.Headers.TryGetValue(sessionTokenHeader, out StringValues sessionToken);
                    if (sessionTokenPresented && sessionToken.Any())
                    {
                        var sessionTokenStr = sessionToken.First();
                        var appPath = httpContext.Request.PathBase.HasValue ? httpContext.Request.PathBase.Value : string.Empty;
                        var returnUrl = string.Format(redirectFrom3DSUrlTemplate, httpContext.Request.Scheme, httpContext.Request.Host, appPath, Convert.ToBase64String(Encoding.UTF8.GetBytes(sessionTokenStr)));
                        requestData.ThreeDSecureRequest = new ThreeDSecureRequest
                        {
                            BrowserAccept = acceptTypes,
                            BrowserUserAgent = userAgent,
                            RemoteIpAddress = ipaddress,
                            TermUrl = returnUrl
                        };
                        paymentFields.Add(new PaymentField
                        {
                            FieldName = "WEB::RETURNURL",
                            FieldValue = returnUrl
                        });
                        paymentFields.Add(new PaymentField
                        {
                            FieldName = "BillTo::IPAddress",
                            FieldValue = ipaddress
                        });
                    }
                }
                requestData.PaymentFields = paymentFields.ToArray();
            }

            // voucher payment
            else if (selectedPayment.PaymentMethodCode == Constants.PaymentMethodCodeVoucher)
            {
                var pendingVoucherPayment = booking.Payments?.ToList().Find(
                    p => p.PaymentMethodType == PaymentMethodType.Voucher 
                    && p.AccountNumber == requestData.AccountNumber 
                    && p.Status == BookingPaymentStatus.Pending);
                if (pendingVoucherPayment != null)
                    throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid,
                            new[] { string.Format("Voucher {0} is already applied", requestData.AccountNumber) });
                GetVoucherInfoResponse voucherInfo;
                try
                {
                    voucherInfo = await paymentsService.GetVoucherInfo(requestData.AccountNumber.Trim());
                }
                catch (System.ServiceModel.FaultException e)
                {
                    throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid,
                        new[] { "Invalid voucher. " + e.Message });
                }
                var voucherAmountAvailable = voucherInfo.getVoucherInfoResponseData.Voucher.Available;
                var voucherCurrencyCode = voucherInfo.getVoucherInfoResponseData.Voucher.CurrencyCode;
                var voucherStatus = voucherInfo.getVoucherInfoResponseData.Voucher.Status;
                var voucherRecLoc = voucherInfo.getVoucherInfoResponseData.Voucher.RecordLocator;
                var voucherFirstName = voucherInfo.getVoucherInfoResponseData.Voucher.FirstName;
                var voucherLastName = voucherInfo.getVoucherInfoResponseData.Voucher.LastName;
                if (voucherStatus != VoucherStatus.Available)
                    throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid, 
                        new[] { "Invalid voucher status: " + voucherStatus.ToString() });
                if (voucherCurrencyCode != booking.CurrencyCode)
                    throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid, 
                        new[] { "Invalid voucher currency: " + voucherCurrencyCode });
                //if (!string.IsNullOrEmpty(voucherRecLoc) && voucherRecLoc != booking.RecordLocator)
                //    throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid, 
                //        new[] { "Invalid voucher. Only redeemable for booking: " + voucherRecLoc });
                if (!string.IsNullOrEmpty(voucherFirstName) && !string.IsNullOrEmpty(voucherLastName))
                {
                    if (booking.Passengers == null || booking.Passengers.ToList().Find(p => p.Names.Length > 0) == null)
                        throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid, 
                            new[] { string.Format("Invalid voucher. Only redeemable for passenger: {0} {1}", voucherFirstName, voucherLastName) });
                    var paxesWithName = booking.Passengers.ToList().FindAll(p => p.Names.Length > 0);
                    var validPax = paxesWithName.Find(
                        p => p.Names[0].FirstName.Equals(voucherFirstName, StringComparison.InvariantCultureIgnoreCase)
                        && p.Names[0].LastName.Equals(voucherLastName, StringComparison.InvariantCultureIgnoreCase));
                    if (validPax == null)
                        throw new ResponseErrorException(ResponseErrorCode.VoucherInvalid,
                            new[] { string.Format("Invalid voucher. Only redeemable for passenger: {0} {1}", voucherFirstName, voucherLastName) });
                }
                requestData.QuotedAmount = voucherAmountAvailable > booking.BookingSum.BalanceDue ? 
                    booking.BookingSum.BalanceDue : voucherAmountAvailable;
                requestData.PaymentVoucher = new PaymentVoucher
                {
                    OverrideAmount = false,
                    OverrideVoucherRestrictions = false,
                    RecordLocator = booking.RecordLocator
                };
                long.TryParse(requestData.AccountNumber, out long vID);
                requestData.PaymentVoucher.VoucherIDField = vID;
                requestData.Expiration = null;
            }
            
            // agency payment
            else if (selectedPayment.PaymentMethodCode == Constants.PaymentMethodCodeAgency)
            {
                requestData.QuotedAmount = booking.BookingSum.BalanceDue;
                requestData.AccountNumber = await sessionBagService.OrganizationCode();
                requestData.Expiration = null;
            }

            // Sadad Online
            else if (selectedPayment.PaymentMethodCode == Constants.PaymentMethodCodeSadadOnline)
            {
                requestData.QuotedAmount = booking.BookingSum.BalanceDue;
                requestData.AccountNumber = "";
                requestData.Expiration = null;

                var httpConnectionFeature = httpContext.Features.Get<IHttpConnectionFeature>();
                var fwdedForHeader = context.HttpContext.Request.Headers["X-Forwarded-For"];
                var ipaddress = fwdedForHeader.Count() > 0 ? fwdedForHeader[0] : httpConnectionFeature?.RemoteIpAddress.ToString();
                ipaddress = !string.IsNullOrEmpty(ipaddress) ? ipaddress.Trim() : "";
                ipaddress = ipaddress == "::1" ? "123.3.53.105" : ipaddress; // if localhost, put in some dummy IP address

                var email = booking.BookingContacts.Length > 0 && !string.IsNullOrEmpty(booking.BookingContacts[0].EmailAddress) ? booking.BookingContacts[0].EmailAddress : "noemail@test.com";
                var phone = booking.BookingContacts.Length > 0 && !string.IsNullOrEmpty(booking.BookingContacts[0].HomePhone) ? booking.BookingContacts[0].HomePhone : "+99999999999";
                //var acceptLanguage = "en";// httpContext.Request.Headers["Accept-Language"]; // en-GB,en-US;q=0.9,en;q=0.8

                var returnUrl = "";
                var redirectFrom3DSUrlTemplate = options.Value.PaymentSettings.RedirectFrom3DSUrlTemplate ?? "";
                if (!string.IsNullOrEmpty(redirectFrom3DSUrlTemplate))
                {
                    var sessionTokenHeader = options.Value.ApplicationSessionOptions.SessionTokenHeader;
                    var sessionTokenPresented = httpContext.Request.Headers.TryGetValue(sessionTokenHeader, out StringValues sessionToken);
                    if (sessionTokenPresented && sessionToken.Any())
                    {
                        var sessionTokenStr = sessionToken.First();
                        var appPath = httpContext.Request.PathBase.HasValue ? httpContext.Request.PathBase.Value : string.Empty;
                        returnUrl = string.Format(redirectFrom3DSUrlTemplate, httpContext.Request.Scheme, httpContext.Request.Host, appPath, Convert.ToBase64String(Encoding.UTF8.GetBytes(sessionTokenStr)));
                    }
                }
                var paymentFields = new List<PaymentField>
                {
                    new PaymentField
                    {
                        FieldName = "CurrencyCode",
                        FieldValue = booking.CurrencyCode
                    },
                    new PaymentField
                    {
                        FieldName = "BillTo::Email",
                        FieldValue = email
                    },
                    new PaymentField
                    {
                        FieldName = "BillTo::IPAddress",
                        FieldValue = ipaddress
                    },
                    //new PaymentField
                    //{
                    //    FieldName = "Browser::AcceptLanguage",
                    //    FieldValue = acceptLanguage
                    //},
                    new PaymentField
                    {
                        FieldName = "BillTo::Phone",
                        FieldValue = phone
                    },
                    new PaymentField
                    {
                        FieldName = "WEB::RETURNURL",
                        FieldValue = returnUrl
                    }
                };
                requestData.PaymentFields = paymentFields.ToArray();
            }

            // other payments
            else
            {
                // balance due is to be paid
                requestData.QuotedAmount = booking.BookingSum.BalanceDue;
                requestData.AccountNumber = "";
                requestData.Expiration = null;
            }
            return requestData;
        }
    }
}
