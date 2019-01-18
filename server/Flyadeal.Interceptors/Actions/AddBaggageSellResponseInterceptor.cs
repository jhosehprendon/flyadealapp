using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Linq;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;
using Flyadeal.Interceptors.Helpers;
using System;
using System.Collections.Generic;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;

namespace Flyadeal.Interceptors.Actions
{

    public class AddBaggageSellResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object result, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var response = result as Newskies.WebApi.Contracts.SellResponse;
            if (response == null)
            {
                return await Task.FromResult(response);
            }
            if (response.BookingUpdateResponseData.Success == null)
            {
                return await Task.FromResult(response);
            }
            var bookingService = context.HttpContext.RequestServices.GetService(typeof(IBookingService)) as BookingService;
            var booking = await bookingService.GetSessionBooking(true);
            if (booking == null || booking.Journeys == null)
            {
                return await Task.FromResult(response);
            }
            var journeys = booking.Journeys.ToList();
            var passengers = booking.Passengers.ToList();
            var ssrService = context.HttpContext.RequestServices.GetService(typeof(ISsrsService)) as SsrsService;
            foreach (var journey in journeys)
            {
                var canMakeChange = string.IsNullOrEmpty(booking.RecordLocator) || (journey.Segments[0].Legs[0].GetUTCDeptDateTime() - Constants.CheckInMinimumPriorToDeparture) > DateTime.UtcNow;
                if (canMakeChange)
                {
                    foreach (var segment in journey.Segments)
                    {
                        var shouldHaveFreeBag15 = segment.Fares.Any(f => f.ProductClass == "CL");
                        var shouldHaveFreeBag25 = segment.Fares.Any(f => f.ProductClass == "YB" || f.ProductClass == "JA" || f.ProductClass == "JB" || f.ProductClass == "CF");
                        foreach (var leg in segment.Legs)
                        {
                            foreach (var passenger in passengers)
                            {
                                var hasFreeBag15 = segment.PaxSSRs.Any(p => p.PassengerNumber == passenger.PassengerNumber && p.SSRCode == Constants.Free15BagSSRCode);
                                var hasFreeBag25 = segment.PaxSSRs.Any(p => p.PassengerNumber == passenger.PassengerNumber && p.SSRCode == Constants.Free25BagSSRCode);
                                var ssrRequestData = new SSRRequestData
                                {
                                    JourneyIndex = journeys.IndexOf(journey),
                                    SegmentIndex = journey.Segments.ToList().IndexOf(segment),
                                    LegIndex = segment.Legs.ToList().IndexOf(leg),
                                    SSRCount = 1,
                                    PaxNumber = passenger.PassengerNumber
                                };

                                // Free Bag 15 kg
                                ssrRequestData.SSRCode = Constants.Free15BagSSRCode;
                                if (shouldHaveFreeBag15 && !hasFreeBag15)
                                {
                                    var sellResponse = await ssrService.SellSSR(new SellSSRRequest
                                    {
                                        SSRRequestData = ssrRequestData
                                    });
                                    HandleBookingUpdateResponseData(sellResponse.BookingUpdateResponseData);                                    
                                }
                                else if (!shouldHaveFreeBag15 && hasFreeBag15)
                                {
                                    var cancelResponse = await ssrService.CancelSSR(new CancelSSRRequest
                                    {
                                        SSRRequestData = ssrRequestData
                                    });
                                    HandleBookingUpdateResponseData(cancelResponse.BookingUpdateResponseData);
                                }

                                // Free Bag 25 kg
                                ssrRequestData.SSRCode = Constants.Free25BagSSRCode;
                                if (shouldHaveFreeBag25 && !hasFreeBag25)
                                {
                                    var sellResponse = await ssrService.SellSSR(new SellSSRRequest
                                    {
                                        SSRRequestData = ssrRequestData
                                    });
                                    HandleBookingUpdateResponseData(sellResponse.BookingUpdateResponseData);
                                }
                                else if (!shouldHaveFreeBag25 && hasFreeBag25)
                                {
                                    var cancelResponse = await ssrService.CancelSSR(new CancelSSRRequest
                                    {
                                        SSRRequestData = ssrRequestData
                                    });
                                    HandleBookingUpdateResponseData(cancelResponse.BookingUpdateResponseData);
                                }
                            }
                        }
                    }
                }
            }
            await bookingService.GetSessionBooking(true);
            return await Task.FromResult(response);
        }

        private void HandleBookingUpdateResponseData(BookingUpdateResponseData bookingUpdateResponseData)
        {
            if (bookingUpdateResponseData.OtherServiceInformations != null && bookingUpdateResponseData.OtherServiceInformations.Length > 0)
            {
                throw new ResponseErrorException(ResponseErrorCode.SellSSRFailure, bookingUpdateResponseData.OtherServiceInformations[0].Text);
            }
            if (bookingUpdateResponseData.Error != null)
            {
                throw new ResponseErrorException(ResponseErrorCode.SellSSRFailure, bookingUpdateResponseData.Error.ErrorText);
            }
            if (bookingUpdateResponseData.Warning != null)
            {
                throw new ResponseErrorException(ResponseErrorCode.SellSSRFailure, bookingUpdateResponseData.Warning.WarningText);
            }
        }
    }
}
