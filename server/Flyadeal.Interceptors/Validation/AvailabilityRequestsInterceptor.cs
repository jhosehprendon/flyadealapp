using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;

namespace Flyadeal.Interceptors.Validation
{
    public class AvailabilityRequestsInterceptor : ArrayLengthInterceptor
    {
        private int _maxAvailabilitySpanDays;

        public AvailabilityRequestsInterceptor() : this(1, 2, 1)
        {
        }

        public AvailabilityRequestsInterceptor(int minAvailabilityRequests = 1, int maxAvailabilityRequests = 2, int maxAvailabilitySpanDays = 1)
            : base(1, 2)
        {
            _maxAvailabilitySpanDays = maxAvailabilitySpanDays;
        }

        public override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var baseResult = base.IsValid(value, validationContext);
            if (baseResult != null)
                return baseResult;
            try
            {
                //var avRequests = value as AvailabilityRequest[];
                //if (avRequests != null)
                //{
                //    if (avRequests.Length > 1)
                //    {
                //        for (var i = 0; i < avRequests.Length - 1; i++)
                //        {
                //            // validate search date order, except for LFF requests
                //            if (avRequests[i].BeginDate > avRequests[i + 1].BeginDate)
                //            {
                //                return new ValidationResult(string.Format("Invalid BeginDate. Journey{0} begin date must be on or after Journey{1} begin date.",
                //                    i + 1, i));
                //            }
                //        }
                //    }
                //}

                var avRequests = value as AvailabilityRequest[];
                if (avRequests != null)
                {
                    if (avRequests.Length > 1)
                    {
                        // validate that currency code is the same for all AvailabilityRequests
                        for (var i = 0; i < avRequests.Length - 1; i++)
                        {
                            if (avRequests[i].CurrencyCode != avRequests[i + 1].CurrencyCode)
                            {
                                return new ValidationResult("Invalid currency code. All AvailabilityRequests must have the same currency code.");
                            }
                        }

                        // validate that first journey currency code equals first journey departure station currency code
                        var appSettings = validationContext.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                        if (!appSettings.Value.AvailabilitySettings.DisableDefaultDepartStationCurrencyValidation)
                        {
                            var validation = ValidationHelper.DefaultCurrencyValidation(avRequests[0].CurrencyCode, validationContext);
                            if (validation != null)
                                return validation;
                        }
                    }
                }

                var requests = value as BaseAvailabilityRequest[];
                foreach (var request in requests)
                {
                    //// validate end dates are after begin dates
                    //if (request.BeginDate > request.EndDate)
                    //    return new ValidationResult(string.Format("Invalid AvailabilityRequest. End date ({0}) must be after begin date ({1}).",
                    //        request.EndDate.ToDateString(), request.BeginDate.ToDateString()));

                    // validate number of days
                    if ((request.EndDate - request.BeginDate).TotalDays + 1 > _maxAvailabilitySpanDays)
                        return new ValidationResult(string.Format("Invalid AvailabilityRequest. Maximum days timespan allowed between begin/end dates is {0}",
                            _maxAvailabilitySpanDays));
                }

                // validate city pairs
                var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
                var markets = Task.Run(async () => await resourcesService.GetMarketList()).Result.MarketList;
                foreach (var req in requests)
                    if (Array.Find(markets, m => !m.InActive && m.LocationCode == req.DepartureStation && m.TravelLocationCode == req.ArrivalStation) == null)
                        return new ValidationResult(string.Format("Invalid market: {0} to {1}",
                            req.DepartureStation, req.ArrivalStation));

                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult("Unable to validate AvailabilityRequests. " + e.Message);
            }
        }
    }
}
