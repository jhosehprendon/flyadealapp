using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class ChangeFlightsRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var changeFlightsRequest = request as ChangeFlightsRequest;
            if (changeFlightsRequest == null || changeFlightsRequest.JourneySellKeys.Length == 0)
                return await Task.FromResult(request);
            changeFlightsRequest.WaiveSeatFee = true;
            changeFlightsRequest.ResellSeatSSRs = true;
            return await Task.FromResult(changeFlightsRequest);
        }
    }
}
