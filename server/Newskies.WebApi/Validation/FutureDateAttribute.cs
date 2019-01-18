using System;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            if (date.Year > DateTime.Now.Year || (date.Year == DateTime.Now.Year && date.Month >= DateTime.Now.Month))
                return ValidationResult.Success;
            return new ValidationResult(string.Format("Invalid {0} date. It must be a future date.", validationContext.MemberName));
        }
    }
}
