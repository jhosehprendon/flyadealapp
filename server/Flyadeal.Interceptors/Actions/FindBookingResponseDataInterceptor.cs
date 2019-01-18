using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class FindBookingResponseDataInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var responseData = response as FindBookingResponseData;
            if (responseData == null)
            {
                return await Task.FromResult(response);
            }

            var bookingDataList = responseData.FindBookingDataList.ToList().Where(c=>c.BookingStatus != BookingStatus.HoldCanceled);
            responseData.FindBookingDataList = bookingDataList.ToArray();

            return await Task.FromResult(responseData);
        }
    }
}
