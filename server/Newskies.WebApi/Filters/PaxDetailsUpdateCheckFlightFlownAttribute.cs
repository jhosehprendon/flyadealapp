using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    /// <summary>
    /// Checks that first flight of a booking has not yet flown.
    /// </summary>
    public class PaxDetailsUpdateCheckFlightFlownAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            var booking = await sessionBagService.Booking();
            if (booking != null)
            {
                var flownSegment = booking.Journeys[0].Segments.ToList().Find(s => s.PaxSegments.ToList().Find(
                    ps => ps.LiftStatus == LiftStatus.Boarded || ps.CheckInStatus == PaxSegmentCheckInStatus.FlightHasAlreadyDeparted) != null);
                if (flownSegment != null)
                {
                    context.Result = new ResponseErrorException(ResponseErrorCode.PassengerDetailsUpdateNotAllowed, "Pax details update not allowed at this time. ").ErrorActionResult();
                }
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
