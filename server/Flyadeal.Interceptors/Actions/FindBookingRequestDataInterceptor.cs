using Flyadeal.Interceptors.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class FindBookingRequestDataInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var requestData = request as FindBookingRequestData;
            if (requestData == null)
            {
                return await Task.FromResult(request);
            }
            requestData.PageSize = Constants.PageSize;
            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            var sessionRoleCode = await sessionBag.RoleCode();
            var sessionAgentId = await sessionBag.AgentId();

            // members
            if (sessionRoleCode == Constants.MemberRoleCode)
            {
                requestData.FindBookingBy = FindBookingBy.ContactCustomerNumber;
                requestData.FindByContactCustomerNumber = new FindByContactCustomerNumber
                {
                    ContactCustomerNumber = await sessionBag.CustomerNumber()
                };
                return await Task.FromResult(requestData);
            }

            // agents
            if (requestData.AgentId > 0)
            {
                var agentService = context.HttpContext.RequestServices.GetService(typeof(IAgentService)) as AgentService;
                var agent = await agentService.GetAgent(requestData.AgentId);
                if (agent != null && (agent.Agent.AgentIdentifier.OrganizationCode != await sessionBag.OrganizationCode() 
                    || (sessionRoleCode == Constants.CorporateSubRoleCode && requestData.AgentId != sessionAgentId)))
                {
                    throw new ResponseErrorException(ResponseErrorCode.AgentUnauthorised,
                        string.Format("Not authorized to retrieve bookings for this account or agent. "));
                }
            }
            if (!string.IsNullOrEmpty(requestData.RecordLocator))
            {
                requestData.FindBookingBy = FindBookingBy.RecordLocator;
                requestData.FindByRecordLocator = new FindByRecordLocator
                {
                    RecordLocator = requestData.RecordLocator,
                    OrganizationCode = await sessionBag.OrganizationCode(),
                    AgentID = sessionRoleCode == Constants.CorporateSubRoleCode ? sessionAgentId : requestData.AgentId
                };
                return await Task.FromResult(requestData);
            }
            if (sessionRoleCode == Constants.AgentMasterRoleCode || sessionRoleCode == Constants.AgentSubRoleCode || 
                sessionRoleCode == Constants.CorporateMasterRoleCode || sessionRoleCode == Constants.CorporateSubRoleCode)
            {
                requestData.FindBookingBy = FindBookingBy.AgencyNumber;
                requestData.FindByAgencyNumber = new FindByAgencyNumber
                {
                    OrganizationCode = await sessionBag.OrganizationCode(),
                    AgentID = sessionRoleCode == Constants.CorporateSubRoleCode ? sessionAgentId : requestData.AgentId
                };
            }
            if (requestData.DepartDate.HasValue || !string.IsNullOrEmpty(requestData.Destination))
            {
                requestData.FindByAgencyNumber.Filter = new Filter
                {
                    DepartureDate = requestData.DepartDate.HasValue ? requestData.DepartDate.Value : new DateTime(),
                    FlightDestination = !string.IsNullOrEmpty(requestData.Destination) ? requestData.Destination : null
                };
            }

            return await Task.FromResult(requestData);
        }
    }
}
