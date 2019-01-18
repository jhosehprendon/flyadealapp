using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using System.Linq;
using Newskies.WebApi.Contracts.Enumerations;
using Flyadeal.Interceptors.Helpers;
using Newskies.WebApi.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class RetrieveBookingResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var retreieveResponse = response as RetrieveBookingResponse;
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as ISessionBagService;
            var bookingService = context.HttpContext.RequestServices.GetService(typeof(IBookingService)) as IBookingService;
            var appSettings = context.HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            var anonymousOrganizationCode = appSettings.Value.NewskiesSettings.AnonymousAgentOrganizationCode;
            if (retreieveResponse == null)
            {
                await bookingService.ClearStateBooking();
                throw new ResponseErrorException(ResponseErrorCode.BookingNotFound, new[] { "Booking not found. " });
            }
            var booking = retreieveResponse.Booking;
            if (booking.BookingInfo.BookingStatus != BookingStatus.Confirmed)
            {
                await bookingService.ClearStateBooking();
                throw new ResponseErrorException(ResponseErrorCode.BookingNotFound, new[] { "Booking not found. " });
            }
            
            // If booking was created by an agent/corp...
            if (booking.SourceBookingPOS.OrganizationCode != anonymousOrganizationCode)
            {
                // And if currently in an anonymous/member session...
                if (await sessionBagService.OrganizationCode() == anonymousOrganizationCode)
                {
                    // And if there are no journey(s) that can be checked in, block retrieval.
                    booking.PopulateCheckInInformation();
                    var checkInAllowed = booking.Journeys.Any(
                        j => j.Segments.Any(s => s.PaxSegments.Any(
                            ps => ps.CheckInStatus.HasValue && ps.CheckInStatus.Value == PaxSegmentCheckInStatus.Allowed)));
                    if (!checkInAllowed)
                    {
                        await bookingService.ClearStateBooking();
                        throw new ResponseErrorException(ResponseErrorCode.BookingNotFound, new[] { "Booking not found. " });
                    }
                }
                // Else currently in an agent/corp session...
                else
                {
                    // Block retrieval if booking was created by another agency/corp
                    if (booking.SourceBookingPOS.OrganizationCode != await sessionBagService.OrganizationCode())
                    {
                        await bookingService.ClearStateBooking();
                        throw new ResponseErrorException(ResponseErrorCode.BookingNotFound, new[] { "Booking not found. " });
                    }
                }
            }

            var lastName = context.HttpContext.Request.Query["lastName"].FirstOrDefault();
            if (!booking.MatchPaxOrContactLastName(lastName))
            {
                await bookingService.ClearStateBooking();
                throw new ResponseErrorException(ResponseErrorCode.BookingNotFound, new[] { "Booking not found. " });
            }
            booking.PopulateCheckInInformation();
            booking.HideSensitivePaymentInformation();
            await sessionBagService.SetBooking(booking);
            booking.BookingComments = null;
            return await Task.FromResult(booking);
        }
    }
}
