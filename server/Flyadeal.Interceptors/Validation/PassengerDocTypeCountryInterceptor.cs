using System;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System.Linq;

namespace Flyadeal.Interceptors.Validation
{
    class PassengerDocTypeCountryInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var passengers = value as Passenger[];
            if (passengers != null)
            {
                foreach (Passenger pax in passengers)
                {
                    foreach (PassengerTravelDocument doc in pax.PassengerTravelDocuments)
                    {
                        if (doc.DocTypeCode.Equals("I", StringComparison.Ordinal) && !doc.IssuedByCode.Equals("SA", StringComparison.Ordinal))
                        {
                            return new ValidationResult("Invalid Issuing country for one or more passengers");
                        }
                        else if (doc.DocTypeCode.Equals("N", StringComparison.Ordinal))
                        {
                            string[] validCountryCodes = { "SA", "AE", "KW", "OM", "BH", "QA" };
                            if (!validCountryCodes.Any(code => doc.IssuedByCode.Equals(code, StringComparison.Ordinal)))
                            {
                                return new ValidationResult("Invalid Issuing country for one or more passengers");
                            }
                        }
                    }
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("PassengerDocTypeCountryInterceptor: Passengers failed null check");
        }
    }
}
