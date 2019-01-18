using Newskies.WebApi.Services;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Linq;
using Newskies.WebApi.Contracts.Enumerations;
using Flyadeal.Interceptors.Helpers;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class GetPostCommitResultsResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var result = response as Newskies.WebApi.Contracts.GetPostCommitResultsResponse;
            if (result == null)
                return await Task.FromResult(result);

            // For SADAD Offline, no need to continue polling as payment status is expected to be pending
            if (await CheckIfSadadOfflinePaymentAndIsReady(result, context))
            {
                result.ShouldContinuePolling = false;
                return await Task.FromResult(result);
            }

            if (result.BookingDelta != null && result.BookingDelta.Payments != null && result.BookingDelta.Payments.Length > 0)
            {
                var lastPayment = result.BookingDelta.Payments.ToList().LastOrDefault();
                if (lastPayment.PaymentFields != null && lastPayment.PaymentFields.Length > 0)
                {
                    result.RedirectPaymentURL = lastPayment.GetPaymentFieldValue("NAVITAIRE$REDIRECTURL");
                    result.RedirectMethod = lastPayment.GetPaymentFieldValue("NAVITAIRE$HTTPMETHOD");
                    if (result.RedirectMethod == "POST")
                    {
                        // sadad online redirect post fields
                        result.RedirectParams = lastPayment.FilterPaymentFields(new List<string>
                        {
                            "payment_option",
                            "return_url",
                            "signature",
                            "language",
                            "merchant_identifier",
                            "merchant_reference",
                            "eci",
                            "command",
                            "currency",
                            "amount",
                            "access_code",
                            "customer_email",
                            "customer_ip",
                            "phone_number"
                        });
                    }
                }
                result.BookingDelta = null;
            }
            return await Task.FromResult(result);
        }

        private async Task<bool> CheckIfSadadOfflinePaymentAndIsReady(Newskies.WebApi.Contracts.GetPostCommitResultsResponse result, ResultExecutingContext context)
        {
            var bookingService = context.HttpContext.RequestServices.GetService(typeof(IBookingService)) as IBookingService;
            if (bookingService == null)
                return false;
            var booking = await bookingService.GetSessionBooking();
            if (booking != null && booking.Payments != null && booking.Payments.Length > 0)
            {
                var lastPayment = booking.Payments.ToList().LastOrDefault();
                if (lastPayment.PaymentMethodCode == Constants.PaymentMethodCodeSadadOffline && lastPayment.Status == BookingPaymentStatus.PendingCustomerAction && lastPayment.AuthorizationStatus == AuthorizationStatus.Pending)
                    return true;
            }
            return false;
        }
    }
}
