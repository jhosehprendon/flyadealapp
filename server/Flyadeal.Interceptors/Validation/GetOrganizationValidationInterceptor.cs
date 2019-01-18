using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Services;
using System;
using Newskies.WebApi.Contracts.Enumerations;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class GetOrganizationValidationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var organizationCode = value as string;
            var sessionBag = validationContext.GetService(typeof(ISessionBagService)) as SessionBagService ?? null;
            if (string.IsNullOrEmpty(organizationCode) || sessionBag == null)
                return new ValidationResult("General validation failure for " + validationContext.DisplayName);
            var sessionOrgCode = Task.Run(async () => await sessionBag.OrganizationCode()).Result;
            if (string.IsNullOrEmpty(sessionOrgCode) || !sessionOrgCode.Equals(organizationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ResponseErrorException(ResponseErrorCode.AgentUnauthorised, string.Format("Retrieval of organization {0} not permitted. ", organizationCode));
            }
            return ValidationResult.Success;
        }
    }
}
