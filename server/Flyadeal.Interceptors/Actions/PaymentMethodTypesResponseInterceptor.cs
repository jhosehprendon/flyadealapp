using Flyadeal.Interceptors.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class PaymentMethodTypesResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object result, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var response = result as Newskies.WebApi.Contracts.GetPaymentMethodsListResponse;
            if (response == null)
            {
                return await Task.FromResult(response);
            }
            if (response.PaymentMethodList == null)
            {
                return await Task.FromResult(response);
            }

            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null)
            {
                return await Task.FromResult(response);
            }
            var booking = await sessionBagService.Booking();
            response.PaymentMethodList = await FilterPaymentMethods(response.PaymentMethodList, sessionBagService, booking, context.HttpContext);
            return await Task.FromResult(response);
        }

        internal static async Task<PaymentMethod[]> FilterPaymentMethods(PaymentMethod[] methods, 
            SessionBagService sessionBagService, Booking booking, HttpContext httpContext)
        {
            if (methods == null)
            {
                return methods;
            }
            var paymentList = methods.ToList();
            var sessionRoleCode = await sessionBagService.RoleCode();
            if (sessionRoleCode.Equals(Constants.AnonymousAgentRoleCode, StringComparison.OrdinalIgnoreCase))
            {
                // remove all methods restricted from WebAnon
                paymentList.RemoveAll(p => !Constants.AmonymousAllowedPaymentMethods.Contains(p.PaymentMethodCode));
            }
            else if (sessionRoleCode.Equals(Constants.AgentMasterRoleCode, StringComparison.OrdinalIgnoreCase) || sessionRoleCode.Equals(Constants.AgentSubRoleCode, StringComparison.OrdinalIgnoreCase))
            {
                // remove all methods restricted from Agents
                paymentList.RemoveAll(p => !Constants.AgencyAllowedPaymentMethods.Contains(p.PaymentMethodCode));
            }
            else if (sessionRoleCode.Equals(Constants.CorporateMasterRoleCode, StringComparison.OrdinalIgnoreCase))
            {
                // remove all methods restricted from master Corporate
                paymentList.RemoveAll(p => !Constants.CorporateMasterAllowedPaymentMethods.Contains(p.PaymentMethodCode));
            }
            else if (sessionRoleCode.Equals(Constants.CorporateSubRoleCode, StringComparison.OrdinalIgnoreCase))
            {
                // remove all methods restricted from sub Corporate
                paymentList.RemoveAll(p => !Constants.CorporateSubAllowedPaymentMethods.Contains(p.PaymentMethodCode));
            }

            // Remove AG for agent/corp if account status is not Open
            if (sessionRoleCode.Equals(Constants.AgentMasterRoleCode, StringComparison.OrdinalIgnoreCase) 
                || sessionRoleCode.Equals(Constants.AgentSubRoleCode, StringComparison.OrdinalIgnoreCase)
                || sessionRoleCode.Equals(Constants.CorporateMasterRoleCode, StringComparison.OrdinalIgnoreCase)
                || sessionRoleCode.Equals(Constants.CorporateSubRoleCode, StringComparison.OrdinalIgnoreCase))
            {
                var accountService = httpContext.RequestServices.GetService(typeof(IAccountService)) as AccountService;
                if (accountService != null)
                {
                    var account = await accountService.GetAccount();
                    if (account == null || account.Status != AccountStatus.Open)
                    {
                        paymentList.RemoveAll(p => p.PaymentMethodCode == Constants.PaymentMethodCodeAgency);
                    }
                }
            }

            if (booking != null && !string.IsNullOrEmpty(booking.RecordLocator))
            {
                // Remove SADAD Offline payment option for existing bookings
                paymentList.RemoveAll(p => p.PaymentMethodCode == Constants.PaymentMethodCodeSadadOffline);
            }
            if (booking != null && booking.Journeys.Length > 0)
            {
                // Remove payments which have a time restriction set in backend
                var deptDatetimeUTC = booking.Journeys[0].Segments[0].GetUTCDeptDateTime();
                paymentList.RemoveAll(p => p.RestrictionHours > 0 && deptDatetimeUTC - DateTime.UtcNow < TimeSpan.FromHours(p.RestrictionHours));

            }
            return paymentList.ToArray();
        }
    }
}
