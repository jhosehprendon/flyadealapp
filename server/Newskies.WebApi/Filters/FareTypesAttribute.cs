using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Constants;
using Newskies.WebApi.Contracts;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class FareTypesAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var options = context.HttpContext.RequestServices.GetService(
                typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            var settings = options != null && options.Value != null ? options.Value.AvailabilitySettings : 
                Global.NonSpecifiedDefaultSettings.AvailabilitySettings;
            foreach (var argument in context.ActionArguments)
            {
                var type = argument.Value.GetType();
                if (type == typeof(TripAvailabilityRequest))
                {
                    var tripAvailabilityRequest = (TripAvailabilityRequest)argument.Value;
                    foreach (var availabilityRequest in tripAvailabilityRequest.AvailabilityRequests)
                        availabilityRequest.FareTypes = settings.FareTypeCodes;
                }
                else if (type == typeof(LowFareTripAvailabilityRequest))
                {
                    var lowFareTripAvailabilityRequest = (LowFareTripAvailabilityRequest)argument.Value;
                    lowFareTripAvailabilityRequest.FareTypeList = settings.FareTypeCodes;
                }
                else if (type == typeof(SellJourneyByKeyRequestData))
                {
                    var sellJourneyByKeyRequestData = (SellJourneyByKeyRequestData)argument.Value;
                    sellJourneyByKeyRequestData.TypeOfSale = new TypeOfSale
                    {
                        PaxResidentCountry = sellJourneyByKeyRequestData.TypeOfSale.PaxResidentCountry,
                        PromotionCode = sellJourneyByKeyRequestData.TypeOfSale.PromotionCode,
                        FareTypes = options.Value.AvailabilitySettings.FareTypeCodes
                    };
                }
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
