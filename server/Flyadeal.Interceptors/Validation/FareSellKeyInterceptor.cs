using Newskies.WebApi.Helpers;
using Newskies.WebApi.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flyadeal.Interceptors.Validation
{
    public class FareSellKeyInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fareSellKey = value as string;
            if (string.IsNullOrEmpty(fareSellKey))
                return ValidationResult.Success; //return new ValidationResult("Missing Fare sell key.");

            var validationErrors = new List<string>();
            if (!NewskiesHelper.IsFareSellKeyValid(fareSellKey, out validationErrors))
            {
                var str = "Fare sell key invalid: ";
                validationErrors.ForEach(p => str += p + " ");
                return new ValidationResult(str.Trim());

            }
            return ValidationResult.Success;
        }
    }
}
