using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Flyadeal.Interceptors.Helpers;
using Newskies.WebApi.Contracts;

namespace Flyadeal.Interceptors.Validation
{
    public class AgentPasswordInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                // Allow empty password for agent update, which is determined by object instance being of type Agent and existence of a Person ID.
                if (validationContext.ObjectInstance is Agent agent && agent.PersonID > 0)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("Could not validate Password, null check failed.");
            }

            Regex exp = new Regex(ValidationHelper.PasswordRegexString);
            if(!exp.IsMatch(password))
            {
                return new ValidationResult("Password must contain 8-16 characters, one upper and lower case letter, one numeric digit, one non-alphanumeric, and no full stops, commas or tildes.");
            }
            return ValidationResult.Success;
        }
    }
}
