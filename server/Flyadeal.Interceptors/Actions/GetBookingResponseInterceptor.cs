using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using Flyadeal.Interceptors.Helpers;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class GetBookingResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var booking = response as Booking;
            if (booking == null)
                return await Task.FromResult(response);

            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as ISessionBagService;
            booking.PopulateCheckInInformation();
            booking.HideSensitivePaymentInformation();
            await sessionBag.SetBooking(booking);
            booking.BookingComments = null;
            return await Task.FromResult(booking);
        }
    }
}
