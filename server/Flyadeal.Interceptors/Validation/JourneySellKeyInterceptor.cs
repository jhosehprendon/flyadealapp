using Newskies.WebApi.Helpers;
using Newskies.WebApi.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flyadeal.Interceptors.Validation
{
    public class JourneySellKeyInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var journeySellKey = value as string;
            if (string.IsNullOrEmpty(journeySellKey))
                return ValidationResult.Success; //return new ValidationResult("Missing Journey sell key.");

            var validationErrors = new List<string>();
            if (!NewskiesHelper.IsJourneySellKeyValid(journeySellKey, out validationErrors))
            {
                var str = "Journey sell key invalid: ";
                validationErrors.ForEach(p => str += p + " ");
                return new ValidationResult(str.Trim());

            }
            return ValidationResult.Success;
        }
    }
}
