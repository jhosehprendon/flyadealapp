using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Flyadeal.Interceptors.Helpers;
using System.Text.RegularExpressions;

namespace Flyadeal.Interceptors.Validation
{
    public class EmailAddressValidationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext context)
        {
            var emailAddress = value as string;
            if (emailAddress == null)
            {
                return new ValidationResult("Failed to validate: Email Address null value");
            }
            if(!Regex.IsMatch(emailAddress, ValidationHelper.EmailRegexString))
            {
                return new ValidationResult("Invalid email address.");
            }
            return ValidationResult.Success;
        }
    }
}
