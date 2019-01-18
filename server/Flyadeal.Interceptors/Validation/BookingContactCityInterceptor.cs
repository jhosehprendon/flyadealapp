using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System;
using Flyadeal.Interceptors.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    class BookingContactCityInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contacts = value as BookingContact[];
            if (contacts == null)
            {
                return new ValidationResult("Contacts failed null check");
            }

            foreach (BookingContact contact in contacts)
            {
                if (!Regex.IsMatch(contact.City, ValidationHelper.CityRegexString))
                {
                    return new ValidationResult(string.Format("Contact City '{0}' should be written in English and not contain special characters or numbers. ", contact.City));
                }
            }
            return ValidationResult.Success;
        }
    }
}
