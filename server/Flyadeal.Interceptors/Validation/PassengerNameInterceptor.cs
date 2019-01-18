using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System;
using Flyadeal.Interceptors.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    class PassengerNameInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorStr = "";

            var passengers = value as Passenger[];
            if(passengers == null)
            {
                return new ValidationResult("PassengerNameInterceptor: Passengers failed null check");
            }

            foreach (Passenger pax in passengers)
            {
                foreach(BookingName name in pax.Names)
                {
                    Regex expression = new Regex(ValidationHelper.NameRegexString);
                    if (!expression.IsMatch(name.FirstName) || !expression.IsMatch(name.LastName))
                    {
                        string fullName = String.Concat(name.FirstName, " ", name.LastName);
                        string newError = String.Concat("Passenger Name '", fullName, "' should be written in English and not contain spaces, special characters or numbers. ");
                        errorStr = String.Concat(errorStr, " ", newError);
                    }
                }
            }

            if(String.IsNullOrEmpty(errorStr))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(errorStr);
        }
    }
}
