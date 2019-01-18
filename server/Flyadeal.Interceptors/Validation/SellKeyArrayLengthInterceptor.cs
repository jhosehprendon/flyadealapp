using dto = Newskies.WebApi.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Navitaire.WebServices.DataContracts.Booking;
using am = AutoMapper;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Configuration;
using Microsoft.Extensions.Options;

namespace Flyadeal.Interceptors.Validation
{
    public class SellKeyArrayLengthInterceptor : ArrayLengthInterceptor
    {
        public SellKeyArrayLengthInterceptor() : base(1, 2)
        {
        }

        public override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var baseResult = base.IsValid(value, validationContext);
            if (baseResult != null)
                return baseResult;
            try
            {
                var airlineSettingsOptions = ((IOptions<AppSettings>)validationContext.GetService(typeof(IOptions<AppSettings>))).Value;
                var bookingSettings = airlineSettingsOptions.AvailabilitySettings;
                var sellKeysList = (dto.SellKeyList[])value;
                if (sellKeysList.Length > 1)
                {
                    var journeysList = new List<Journey>();
                    foreach (var sellKey in sellKeysList)
                        journeysList.Add(
                            NewskiesHelper.CreateJourney(am.Mapper.Map<SellKeyList>(sellKey)));
                    for (var i = 0; i < journeysList.Count - 1; i++)
                    {
                        if (journeysList[i].GetJourneySTA().AddHours(bookingSettings.MinHoursBetweenJourneys)
                            > journeysList[i + 1].GetJourneySTD())
                            return new ValidationResult(string.Format("{0} error. Journey {1} departure must be after journey {2} arrival plus {3} hour(s).",
                                validationContext.DisplayName, i + 1, i, bookingSettings.MinHoursBetweenJourneys));
                    }
                }

                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult(string.Format("Unable to validate {0} array length. {1}", validationContext.DisplayName, e.Message));
            }
        }
    }
}
