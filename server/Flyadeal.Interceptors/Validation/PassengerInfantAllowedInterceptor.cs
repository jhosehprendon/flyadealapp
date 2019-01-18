using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Contracts;
using Flyadeal.Interceptors.Helpers;
using System;
using Newskies.WebApi.Constants;

namespace Flyadeal.Interceptors.Validation
{
    class PassengerInfantAllowedInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext context)
        {
            var passengers = value as Passenger[];

            var hasAdult = false;
            var hasInfant = false;
            var hasChild = false;
            foreach(Passenger pax in passengers)
            {
                if (pax.Infant != null) hasInfant = true;
                if (pax.PassengerTypeInfo.PaxType.Equals(Global.CHILD_CODE)) hasChild = true;
                var age = ValidationHelper.GetAge(DateTime.UtcNow, pax.PassengerTypeInfo.DOB);
                if (!hasAdult && age >= 16) hasAdult = true;
            }
            if (hasInfant || hasChild)
            {
                if (!hasAdult) return new ValidationResult("Invalid Infant or Child Passenger - Booking must have one or more adults (16+) to allow minors.");
            }

            return ValidationResult.Success;
        }
    }
}
