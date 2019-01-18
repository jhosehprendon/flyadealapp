using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newskies.WebApi.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Flyadeal.Interceptors.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    public class RetrieveBookingRequestInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectType == typeof(RetrieveBookingRequest))
            {
                var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                var query = httpContextAccessor.HttpContext.Request.Query;
                var lastName = query.ContainsKey("lastName") ? query["lastName"].First() : "";
                if (string.IsNullOrEmpty(lastName))
                    return new ValidationResult("LastName is required. ");
                if (!Regex.IsMatch(lastName, ValidationHelper.NameRegexString))
                    return new ValidationResult("Invalid lastName: " + lastName);
            }
            return ValidationResult.Success;
        }
    }
}
