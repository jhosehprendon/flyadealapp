using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using System.Linq;

namespace Flyadeal.Interceptors.Actions
{
    public class AutoAssignSeatsResponseInterceptor : IResponseInterceptor
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
            var seatService = context.HttpContext.RequestServices.GetService(typeof(ISeatsService)) as SeatsService;
            for (var j = 0; j < booking.Journeys.Length; j++)
            {
                for (var s = 0; s < booking.Journeys[j].Segments.Length; s++)
                {
                    for (var l = 0; l < booking.Journeys[j].Segments[s].Legs.Length; l++)
                    {
                        // new booking
                        if (string.IsNullOrEmpty(booking.RecordLocator))
                        {
                            response.BookingUpdateResponseData = await AssignSeat(seatService, j, s, l);
                        }

                        // changing existing booking
                        else
                        {
                            var segment = booking.Journeys[j].Segments[s];
                            var leg = segment.Legs[l];
                            for (short p = 0; p < booking.Passengers.Length; p++)
                            {
                                var existingSeat = segment.PaxSeats.ToList().Find(
                                    ps => ps.PassengerNumber == p && ps.DepartureStation == leg.DepartureStation && ps.ArrivalStation == leg.ArrivalStation);
                                if (existingSeat == null)
                                {
                                    response.BookingUpdateResponseData = await AssignSeat(seatService, j, s, l, p);
                                }
                            }
                        }
                    }
                }
            }
            await bookingService.GetSessionBooking(true);
            return await Task.FromResult(response);
        }

        private async Task<BookingUpdateResponseData> AssignSeat(SeatsService seatService, int journeyIndex, int segmentIndex, int legIndex, short? paxNumber = null)
        {
            var assignResponse = await seatService.AssignSeat(new AssignSeatRequest
            {
                AssignSeatData = new AssignSeatData
                {
                    JourneyIndex = journeyIndex,
                    SegmentIndex = segmentIndex,
                    LegIndex = legIndex,
                    PaxNumber = paxNumber,
                    WaiveFees = false
                }
            });
            if (assignResponse.BookingUpdateResponseData.Error != null)
            {
                throw new ResponseErrorException(ResponseErrorCode.SeatAssignmentFailure, assignResponse.BookingUpdateResponseData.Error.ErrorText);
            }
            return assignResponse.BookingUpdateResponseData;
        }
    }
}
