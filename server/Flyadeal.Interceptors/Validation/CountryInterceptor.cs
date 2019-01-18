using System;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Services;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class CountryInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var countryCode = value as string ?? "";
            countryCode = countryCode.Trim();
            if (string.IsNullOrEmpty(countryCode))
                return new ValidationResult("Country required.");
            if (countryCode.Length != 2)
                return new ValidationResult("Country must be 2 characters in length.");
            var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
            try
            {
                var response = Task.Run(async () => await resourcesService.GetCountryList()).Result;
                var country = Array.Find(response.CountryList, c => c.CountryCode.Equals(countryCode));
                if (country == null)
                    return new ValidationResult(string.Format("Invalid country: {0}.", countryCode));
                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult("Unable to validate country. " + e.Message);
            }
        }
    }
}
