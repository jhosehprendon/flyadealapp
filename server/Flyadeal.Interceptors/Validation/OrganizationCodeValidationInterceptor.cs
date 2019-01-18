using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Flyadeal.Interceptors.Validation
{
    public class OrganizationCodeValidationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var orgCode = value as string;
            var org = validationContext.ObjectInstance as Organization;
            if (org == null)
            {
                return ValidationResult.Success;
            }
            if (org.OrganizationType == OrganizationType.TravelAgency)
            {
                if (string.IsNullOrEmpty(orgCode) || !Regex.IsMatch(orgCode.Trim(), "^[0-9]{7,10}$"))
                {
                    return new ValidationResult("Organization Code must be 7 to 10 numeric characters in length. ");
                }
            }
            return ValidationResult.Success;
        }
    }
}
