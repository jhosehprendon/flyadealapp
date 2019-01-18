using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Threading.Tasks;
using Flyadeal.Interceptors.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Flyadeal.Interceptors.Actions
{
    public class AddSMSFeeSellResponseResponseInterceptor : IResponseInterceptor { 
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
            // only add SMS fee for new bookings
            if (!string.IsNullOrEmpty(booking.RecordLocator))
            {
                return await Task.FromResult(response);
            }
            if (booking.Passengers == null || booking.Passengers.Length == 0)
            {
                return await Task.FromResult(response);
            }
            var smsFeeExists = booking.Passengers.ToList().Find(p => p.PassengerFees.ToList().Find(f => f.FeeCode == Constants.FeeCodeSMS) != null) != null;
            if (smsFeeExists)
            {
                return await Task.FromResult(response);
            }
            var feeService = context.HttpContext.RequestServices.GetService(typeof(IFeeService)) as FeeService;
            await feeService.SellFee(new Newskies.WebApi.Contracts.SellFeeRequestData
            {
                FeeCode = Constants.FeeCodeSMS,
                PassengerNumber = 0
            });
            await bookingService.GetSessionBooking(true); //override the local sync check.
            return await Task.FromResult(response);
        }
    }
}
