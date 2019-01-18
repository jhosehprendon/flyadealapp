using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class GetAllResourcesResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object result, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var response = result as Newskies.WebApi.Contracts.AllResourcesResponse;
            if (response == null)
            {
                return await Task.FromResult(response);
            }
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null)
            {
                return await Task.FromResult(response);
            }
            var booking = await sessionBagService.Booking();
            response.DocTypeList = GetDocTypeListResponseInterceptor.FilterDocumentTypes(booking, response.DocTypeList);
            response.PaymentMethodList = await PaymentMethodTypesResponseInterceptor.FilterPaymentMethods(response.PaymentMethodList, sessionBagService, booking, context.HttpContext);
            return await Task.FromResult(response);
        }
    }
}
