using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Helpers;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;

namespace Flyadeal.Interceptors.Validation
{
    public class CurrencyCodeInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var appSettings = validationContext.GetService(
                typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            if (appSettings != null && appSettings.Value != null && appSettings.Value.AvailabilitySettings.DisableDefaultDepartStationCurrencyValidation)
                return ValidationResult.Success; var currencyCode = value as string ?? "";

            currencyCode = currencyCode.ToUpper().Trim();
            return ValidationHelper.DefaultCurrencyValidation(currencyCode, validationContext);
        }
    }
}
