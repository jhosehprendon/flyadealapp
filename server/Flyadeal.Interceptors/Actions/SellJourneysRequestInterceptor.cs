using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class SellJourneysRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBag == null || await sessionBag.Booking() == null)
            {
                return await Task.FromResult(request);
            }
            var booking = await sessionBag.Booking();
            var existingBookingFlightCount = !string.IsNullOrEmpty(booking.RecordLocator) && booking.Journeys != null ? booking.Journeys.Length : 0;
            var sellJourneyByKeyRequestData = request as SellJourneyByKeyRequestData;
            if (sellJourneyByKeyRequestData != null && sellJourneyByKeyRequestData.JourneySellKeys != null)
            {
                var sellKeys = sellJourneyByKeyRequestData.JourneySellKeys;
                var flightCount = existingBookingFlightCount + sellKeys.Length;
                if (flightCount > 2)
                {
                    throw new ResponseErrorException(ResponseErrorCode.InvalidFlightsLimit, "Maximum 2 flights allowed. ");
                }
                if (flightCount == 2)
                {
                    // disallow multi-city
                    if (existingBookingFlightCount == 0 && sellKeys.Length == 2)
                    {
                        // no flights sold yet and requesting to sell 2 flights
                        if (!NewskiesHelper.IsJourneySellKeyValid(sellKeys[0].JourneySellKey, out List<string> errors))
                        {
                            throw new ResponseErrorException(ResponseErrorCode.InvalidRequest, errors.ToArray());
                        }
                        if (!NewskiesHelper.IsJourneySellKeyValid(sellKeys[1].JourneySellKey, out List<string> errors2))
                        {
                            throw new ResponseErrorException(ResponseErrorCode.InvalidRequest, errors2.ToArray());
                        }
                        var f1Array = sellKeys[0].JourneySellKey.Split(new char[] { '~' });
                        var f1Orig = f1Array[4];
                        var f1Dest = f1Array[6];
                        var f2Array = sellKeys[1].JourneySellKey.Split(new char[] { '~' });
                        var f2Orig = f2Array[4];
                        var f2Dest = f2Array[6];
                        if (f1Orig != f2Dest || f1Dest != f2Orig)
                        {
                            throw new ResponseErrorException(ResponseErrorCode.InvalidMarket, "Multi-city prohibited. ");
                        }
                    }
                    else if (existingBookingFlightCount == 1 && sellKeys.Length == 1)
                    {
                        // 1 flight already sold and requesting to sell second flight
                        var f1Orig = booking.Journeys[0].GetDepartureStation();
                        var f1Dest = booking.Journeys[0].GetArrivalStation();
                        var f2Array = sellKeys[0].JourneySellKey.Split(new char[] { '~' });
                        var f2Orig = f2Array[4];
                        var f2Dest = f2Array[6];
                        if (f1Orig != f2Dest || f1Dest != f2Orig)
                        {
                            throw new ResponseErrorException(ResponseErrorCode.InvalidMarket, "Multi-city prohibited. ");
                        }
                    }
                }
                return await Task.FromResult(request);
            }
            return await Task.FromResult(request);
        }
    }
}
