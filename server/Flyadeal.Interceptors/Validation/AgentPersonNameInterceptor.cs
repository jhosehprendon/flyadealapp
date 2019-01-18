using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Contracts;
//using Newskies.WebApi.Services;
using System.Text.RegularExpressions;
using Flyadeal.Interceptors.Helpers;
using System;

namespace Flyadeal.Interceptors.Validation
{
    public class AgentPersonNameInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var agentName = value as Name;
            if(agentName == null)
            {
                return new ValidationResult("Could not validate Agent Name, null check failed.");
            }
            // TODO: Validate Title
            /*var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService ?? null;
            if(resourcesService == null)
            {
                return new ValidationResult("General validation failure for " + validationContext.DisplayName);
            }*/
            Regex expression = new Regex(ValidationHelper.NameRegexString);
            if(!expression.IsMatch(agentName.FirstName) || !expression.IsMatch(agentName.LastName))
            {
                string fullName = String.Concat(agentName.FirstName, " ", agentName.LastName);
                return new ValidationResult(String.Concat("Agent Name '", fullName, "' should be written in English and not contain spaces, special characters or numbers. "));
            }

            return ValidationResult.Success;
        }
    }
}

