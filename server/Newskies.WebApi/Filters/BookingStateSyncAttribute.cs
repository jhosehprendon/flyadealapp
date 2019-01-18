using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;

namespace Newskies.WebApi.Filters
{
    /// <summary>
    /// This action filter attribute is to be used when a controller action is expected to make a change in the Booking State on
    /// Navitaire's side. If the context's result is of type OkObjectResult, it is assumed that the user's request was successful
    /// therefore the Booking State at Navitaire's side will have changed and become different to the Booking in local session.
    /// Hence, ISessionBagService.BookingStateInSync is set to false at the end of context execution. See BookingService.GetSessionBooking()
    /// on details of its reliance on ISessionBagService.BookingStateInSync.
    /// </summary>
    public class BookingStateSyncAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as ISessionBagService;
            var result = context.Result as OkObjectResult;
            if (result != null)
                sessionBagService.SetBookingStateNotInSync().Wait();
            base.OnResultExecuted(context);
        }
    }
}
