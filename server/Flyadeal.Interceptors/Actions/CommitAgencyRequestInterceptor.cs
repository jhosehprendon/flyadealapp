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
    public class CommitAgencyRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var commitAgencyRequest = request as CommitAgencyRequest;
            if (commitAgencyRequest == null)
                return await Task.FromResult(request);
            var agencyService = context.HttpContext.RequestServices.GetService(typeof(IAgencyService)) as AgencyService;
            if (agencyService == null)
                return await Task.FromResult(request);
            commitAgencyRequest.Organization.OrganizationCode = await DetermineNewOrgCode(commitAgencyRequest, agencyService);
            //commitAgencyRequest.Organization.OrganizationType = OrganizationType.TravelAgency;
            commitAgencyRequest.Organization.CurrencyCode = "SAR";
            commitAgencyRequest.Organization.Status = OrganizationStatus.Pending;
            commitAgencyRequest.CommitAgentRequestData.Agent.Status = AgentStatus.Pending;
            commitAgencyRequest.CommitAgentRequestData.Agent.LoginName = commitAgencyRequest.CommitAgentRequestData.Agent.LoginName.ToLower().Trim();
            commitAgencyRequest.CommitAgentRequestData.Agent.AgentIdentifier = new AgentIdentifier
            {
                AgentName = commitAgencyRequest.CommitAgentRequestData.Agent.LoginName
            };
            commitAgencyRequest.CommitAgentRequestData.Agent.AgentRoles = new AgentRole[]
            {
                new AgentRole
                {
                    RoleCode = commitAgencyRequest.Organization.OrganizationType == OrganizationType.TravelAgency ? Constants.AgentMasterRoleCode : Constants.CorporateMasterRoleCode,
                    EffectiveDate = DateTime.UtcNow.AddDays(-1),
                    EndEffectiveDate = NewskiesHelper.DATE_TIME_MAX_VALUE,
                    EffectiveDOW = DOW.Daily
                }
            };
            commitAgencyRequest.CommitAgentRequestData.Agent.AgentIdentifier.DomainCode = Constants.DomainCode;
            commitAgencyRequest.CommitAgentRequestData.Agent.DepartmentCode = Constants.DepartmentCode;
            commitAgencyRequest.CommitAgentRequestData.Agent.LocationCode = Constants.LocationCode;
            commitAgencyRequest.CommitAgentRequestData.Person.PersonType = PersonType.Agent;
            return await Task.FromResult(commitAgencyRequest);
        }

        private async Task<string> DetermineNewOrgCode(CommitAgencyRequest commitAgencyRequest, AgencyService agencyService)
        {
            if (commitAgencyRequest.Organization.OrganizationType != OrganizationType.ThirdParty)
            {
                return commitAgencyRequest.Organization.OrganizationCode.Trim();
            }
            var str = commitAgencyRequest.Organization.OrganizationName.Trim().ToUpper().Replace(" ", "");
            var str2 = string.Empty;
            if (str.Length >= 7)
            {
                str2 = str.Substring(0, 7);
            }
            else
            {
                str2 = str;
                for (var i = str.Length; i < 7; i++)
                {
                    str2 += "0";
                }
            }

            var suffixNumber = 1;
            var str3 = string.Empty;
            var keepTrying = true;
            while (keepTrying && suffixNumber <= 100)
            {
                try
                {
                    str3 = str2 + suffixNumber.ToString();
                    var result = await agencyService.GetAgency(new GetOrganizationRequestData
                    {
                        OrganizationCode = str3
                    });
                    if (result == null || result.Organization == null)
                    {
                        keepTrying = false;
                    }
                }
                catch {}
                finally { suffixNumber++; }
            }
            if (keepTrying)
            {
                throw new ResponseErrorException(ResponseErrorCode.OrganizationCodeGenerationFailure, "Failed to generate Organization Code for third party organization. ");
            }
            return str3;
        }
    }
}
