using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Flyadeal.Interceptors.Helpers;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class CheckInPassengersResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var result = response as Newskies.WebApi.Contracts.CheckInPassengersResponse;
            if (result == null)
                return await Task.FromResult(response);
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null || (await sessionBagService.Booking()) == null)
            {
                return await Task.FromResult(response);
            }
            var queueService = context.HttpContext.RequestServices.GetService(typeof(IQueueService)) as IQueueService;
            await queueService.AddBookingToQueue(Constants.WebCheckInBookingQueueCode, "WCI");
            return await Task.FromResult(response);
        }
    }
}
