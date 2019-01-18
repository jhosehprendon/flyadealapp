using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Contracts;
using Flyadeal.Interceptors.Helpers;
using Newskies.WebApi.Services;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class AgentRolesInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var agentRoles = value as AgentRole[];
            if (agentRoles == null || agentRoles.Length == 0)
                return ValidationResult.Success;
            var sessionBagService = validationContext.GetService(typeof(ISessionBagService)) as SessionBagService ?? null;
            if (sessionBagService == null)
                return new ValidationResult("General validation failure for " + validationContext.DisplayName);

            if (validationContext.ObjectType == typeof(Agent) && !ValidationHelper.IsRoleCodeForNewAgentValid(Task.Run(async () => await sessionBagService.RoleCode()).Result, agentRoles[0].RoleCode))
                return new ValidationResult("Invalid role code for agent: " + agentRoles[0].RoleCode);
            return ValidationResult.Success;

        }
    }
}
