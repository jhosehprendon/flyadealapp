using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts.Enumerations;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class GetBarCodedBoardingPassesRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var getBarCodedBoardingPassesRequest = request as Newskies.WebApi.Contracts.GetBarCodedBoardingPassesRequest;
            if (getBarCodedBoardingPassesRequest == null || getBarCodedBoardingPassesRequest.BoardingPassRequest == null)
                return await Task.FromResult(request);
            getBarCodedBoardingPassesRequest.BoardingPassRequest.BarCodeType = BarCodeType.M2D;
            return await Task.FromResult(getBarCodedBoardingPassesRequest);
        }
    }
}
