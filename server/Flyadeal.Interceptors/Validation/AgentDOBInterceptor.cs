using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Flyadeal.Interceptors.Helpers;
using System;

namespace Flyadeal.Interceptors.Validation
{
    class AgentDOBInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dob = Convert.ToDateTime(value);
            if(dob == null)
            {
                return new ValidationResult("Could not validate Member DOB, null check failed");
            }
            var age = ValidationHelper.GetAge(DateTime.UtcNow, dob);
            if (age < ValidationHelper.AdultMinAge || age > ValidationHelper.AdultMaxAge)
            {
                return new ValidationResult("Member Date of Birth invalid, Member must be of adult age.");
            }

            return ValidationResult.Success;
        }
    }
}
