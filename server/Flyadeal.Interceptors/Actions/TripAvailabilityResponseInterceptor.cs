using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    class TripAvailabilityResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var availResponse = response as TripAvailabilityResponse;
            if (availResponse == null)
            {
                return await Task.FromResult(response);
            }

            foreach (JourneyDateMarket[] schedules in availResponse.Schedules)
            {
                foreach (JourneyDateMarket schedule in schedules)
                {
                    foreach (Journey journey in schedule.Journeys)
                    {
                        foreach (Segment segment in journey.Segments)
                        {
                            foreach (AvailableFare2 fare in segment.AvailableFares)
                            {
                                if (fare.AvailableCount > 9)
                                {
                                    fare.AvailableCount = short.MaxValue;
                                }
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(availResponse);
        }
    }
}
