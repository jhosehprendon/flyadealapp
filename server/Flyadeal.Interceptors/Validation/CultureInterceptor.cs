using System;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Services;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class CultureInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cultureCode = value as string ?? "";
            cultureCode = cultureCode.Trim();
            if (string.IsNullOrEmpty(cultureCode))
                return new ValidationResult("Culture required.");
            var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
            try
            {
                var response = Task.Run(async () => await resourcesService.GetCultureList()).Result;
                var culture = Array.Find(response.CultureList, c => c.Code.Equals(cultureCode));
                if (culture == null)
                    return new ValidationResult(string.Format("Invalid culture: {0}.", cultureCode));
            }
            catch (Exception e)
            {
                return new ValidationResult("Unable to validate culture. " + e.Message);
            }
            return ValidationResult.Success;
        }
    }
}
