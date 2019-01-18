using AutoMapper;
using Microsoft.Extensions.Options;
using Navitaire.WebServices.DataContracts.Common;
using Newskies.PersonManager;
using Newskies.WebApi.Configuration;
using dto = Newskies.WebApi.Contracts;
using System;
using System.Threading.Tasks;
using Newskies.AgentManager;
using Navitaire.WebServices.DataContracts.Common.Navitaire.WebServices.DataContracts.Common.Enumerations;

namespace Newskies.WebApi.Services
{
    public interface IAgencyService
    {
        Task<dto.CommitAgencyResponse> AddAgency(dto.CommitAgencyRequest commitAgencyRequest);
        Task<dto.GetOrganizationResponse> GetAgency(dto.GetOrganizationRequestData requestData);
    }
    public class AgencyService : ServiceBase, IAgencyService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IAgentManager _agentClient;
        private readonly IPersonManager _personClient;
        private readonly IUserSessionService _userSessionService;
        private readonly IAgentService _agentService;

        public AgencyService(ISessionBagService sessionBag, IAgentManager agentClient, IPersonManager personClient, 
            IUserSessionService userSessionService, IAgentService agentService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _agentService = agentService ?? throw new ArgumentNullException(nameof(agentService));
            _agentClient = agentClient ?? throw new ArgumentNullException(nameof(agentClient));
            _personClient = personClient ?? throw new ArgumentNullException(nameof(personClient));
        }

        public async Task<dto.GetOrganizationResponse> GetAgency(dto.GetOrganizationRequestData requestData)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            GetOrganizationResponse response = await _agentClient.GetOrganizationAsync(new GetOrganizationRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                GetOrganizationReqData = new GetOrganizationRequestData
                {
                    OrganizationCode = requestData.OrganizationCode.ToUpper()
                }
            });
            return new dto.GetOrganizationResponse
            {
                Organization = Mapper.Map<dto.Organization>(response.Organization)
            };
        }

        public async Task<dto.CommitAgencyResponse> AddAgency(dto.CommitAgencyRequest commitAgencyRequest)
        {
            if (!string.IsNullOrEmpty(await _sessionBag.AgentName()))
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AlreadyLoggedIn,
                    string.Format("Unable to create agency, Logged in as {0}", await _sessionBag.AgentName()));
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();

            // check if organization already exists
            GetOrganizationResponse getResponse = null;
            try
            {
                getResponse = await _agentClient.GetOrganizationAsync(new GetOrganizationRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetOrganizationReqData = new GetOrganizationRequestData
                    {
                        OrganizationCode = commitAgencyRequest.Organization.OrganizationCode.ToUpper()
                    }
                });                
            }
            catch { } 
            if (getResponse != null && getResponse.Organization != null)
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AgencyAlreadyExists,
                    string.Format("Agency {0} already exists: {1}.", getResponse.Organization.OrganizationCode, getResponse.Organization.OrganizationName));

            // check if agent already exists (by login name)
            dto.FindAgentsResponse findAgentResponse = null;
            try
            {
                findAgentResponse = await _agentService.FindAgent(new dto.FindAgentRequestData
                {
                    AgentName = new dto.SearchString
                    {
                        SearchType = dto.Enumerations.SearchType.ExactMatch,
                        Value = commitAgencyRequest.CommitAgentRequestData.Agent.LoginName.ToLower()
                    },
                    DomainCode = _newskiesSettings.AgentDomain
                });
            }
            catch { }
            if (findAgentResponse != null && findAgentResponse.FindAgentResponseData != null &&
                findAgentResponse.FindAgentResponseData.FindAgentList != null &&
                findAgentResponse.FindAgentResponseData.FindAgentList.Length != 0)
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AgentAlreadyExists,
                    string.Format("Agent login name'{0}' already exists. ", commitAgencyRequest.CommitAgentRequestData.Agent.LoginName.ToLower()));

            // create organization
            var commitOrganizationResp = await _agentClient.CommitOrganizationAsync(new CommitOrganizationRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                Organization = Mapper.Map<Organization>(commitAgencyRequest.Organization)
            });

            // create master agent
            var mappedAgentRequest = Mapper.Map<CommitAgentRequestData>(commitAgencyRequest.CommitAgentRequestData);
            mappedAgentRequest.Agent.AgentIdentifier.OrganizationCode = commitOrganizationResp.Organization.OrganizationCode;
            mappedAgentRequest.Agent.AuthenticationType = AuthenticationType.Password;
            var commitAgentResponse = await _agentClient.CommitAgentAsync(new CommitAgentRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                CommitAgentReqData = mappedAgentRequest
            });

            return new dto.CommitAgencyResponse
            {
                Organization = Mapper.Map<dto.Organization>(commitOrganizationResp.Organization),
                CommitAgentResponseData = Mapper.Map<dto.CommitAgentResponseData>(commitAgentResponse.CommitAgentResData)
            };

        }
    }
}
