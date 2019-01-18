using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System;
using Flyadeal.Interceptors.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    class BookingContactNameInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorStr = "";

            var contacts = value as BookingContact[];
            if (contacts == null)
            {
                return new ValidationResult(String.Format("{0} Passengers failed null check", validationContext.DisplayName));
            }

            foreach (BookingContact contact in contacts)
            {
                foreach (BookingName name in contact.Names)
                {
                    Regex expression = new Regex(ValidationHelper.NameRegexString);
                    if (!expression.IsMatch(name.FirstName) || !expression.IsMatch(name.LastName))
                    {
                        string fullName = String.Concat(name.FirstName, " ", name.LastName);
                        string newError = String.Concat("Contact Name '", fullName , "' should be written in English and not contain spaces, special characters or numbers. ");
                        errorStr = String.Concat(errorStr, " ", newError);
                    }
                }
            }

            if (String.IsNullOrEmpty(errorStr))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(errorStr);
        }
    }
}
