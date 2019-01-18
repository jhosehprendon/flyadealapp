using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Flyadeal.Interceptors.Helpers;
using System;
using Newskies.WebApi.Helpers;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    class CommitAgentRequestDataRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var requestData = request as CommitAgentRequestData;
            if (requestData == null)
                return await Task.FromResult(request);
            var httpContext = context.HttpContext;
            var sessionBagService = httpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null)
                return await Task.FromResult(request);
            var sessionRoleCode = await sessionBagService.RoleCode();
            var sessionOrganizationCode = await sessionBagService.OrganizationCode();
            var newAgentRoleCode = requestData.Agent.AgentRoles != null && requestData.Agent.AgentRoles.Length > 0 ? requestData.Agent.AgentRoles[0].RoleCode : null;
            if (string.IsNullOrEmpty(newAgentRoleCode) || !Helpers.ValidationHelper.IsRoleCodeForNewAgentValid(sessionRoleCode, newAgentRoleCode))
                throw new ResponseErrorException(ResponseErrorCode.InvalidAgentRoleCode, new[] { "Missing or invalid agent role code. " });
            requestData.Agent.LoginName = requestData.Agent.LoginName.ToLower().Trim();
            requestData.Agent.AgentIdentifier = new AgentIdentifier
            {
                AgentName = requestData.Agent.LoginName
            };
            requestData.Agent.AgentRoles = new AgentRole[]
            {
                new AgentRole
                {
                    RoleCode = newAgentRoleCode,
                    EffectiveDate = DateTime.UtcNow.AddDays(-1),
                    EndEffectiveDate = NewskiesHelper.DATE_TIME_MAX_VALUE,
                    EffectiveDOW = DOW.Daily
                }
            };
            requestData.Agent.AgentIdentifier.DomainCode = Constants.DomainCode;
            requestData.Agent.AgentIdentifier.OrganizationCode = sessionOrganizationCode;
            requestData.Agent.DepartmentCode = Constants.DepartmentCode;
            requestData.Agent.LocationCode = Constants.LocationCode;

            // Member
            if (newAgentRoleCode == Constants.MemberRoleCode)
            {
                requestData.Person.PersonType = PersonType.Customer;
                requestData.Agent.Status = AgentStatus.Active;
            }

            // Sub Agent
            else if (newAgentRoleCode == Constants.AgentSubRoleCode)
            {
                requestData.Person.PersonType = PersonType.Agent;
                requestData.Agent.Status = AgentStatus.Active;
            }

            // Master Agent
            else if (newAgentRoleCode == Constants.AgentMasterRoleCode)
            {
                requestData.Person.PersonType = PersonType.Agent;
                requestData.Agent.Status = AgentStatus.Active;
            }

            // Sub Corporate
            else if (newAgentRoleCode == Constants.CorporateSubRoleCode)
            {
                requestData.Person.PersonType = PersonType.Agent;
                requestData.Agent.Status = AgentStatus.Active;
            }

            // Master Corporate
            else if (newAgentRoleCode == Constants.CorporateMasterRoleCode)
            {
                requestData.Person.PersonType = PersonType.Agent;
                requestData.Agent.Status = AgentStatus.Active;
            }

            return requestData;
        }
    }
}
