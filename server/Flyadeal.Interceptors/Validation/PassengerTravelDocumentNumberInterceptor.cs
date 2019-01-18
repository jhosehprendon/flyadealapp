using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flyadeal.Interceptors.Validation
{
    class PassengerTravelDocumentNumberInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext context)
        {
            var passengers = value as Passenger[];
            if(passengers == null)
            {
                return new ValidationResult(string.Format("Failed to validate {0}. Passengers failed null check", context.DisplayName));
            }

            List<string> ids = new List<string>();
            foreach (Passenger pax in passengers)
            {
                foreach (PassengerTravelDocument doc in pax.PassengerTravelDocuments)
                {
                    ids.Add(doc.DocNumber);
                }
            }
            
            if(ids.Count != ids.Distinct().Count())
            {
                return new ValidationResult("Failed to validate: One or more Travel Documents are using the same ID.");
            }

            return ValidationResult.Success;
        }
    }
}
