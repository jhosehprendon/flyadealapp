using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Flyadeal.Interceptors.Helpers;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class ItineraryResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var result = (bool)response;
            if (response == null || !result)
                return await Task.FromResult(response);
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null || (await sessionBagService.Booking()) == null)
            {
                return await Task.FromResult(response);
            }
            var queueService = context.HttpContext.RequestServices.GetService(typeof(IQueueService)) as IQueueService;
            await queueService.AddBookingToQueue(Constants.ItineraryBookingQueueCode, "MMB");
            return await Task.FromResult(response);
        }
    }
}
