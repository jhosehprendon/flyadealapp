using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Newskies.WebApi.Contracts;
using nsk = Newskies.SessionManager;
using nskam = Newskies.AgentManager;
using Microsoft.AspNetCore.Http;
using Newskies.WebApi.Contracts.Enumerations;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Configuration;
using Navitaire.WebServices.DataContracts.Common;
using Navitaire.WebServices.DataContracts.Common.Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.PersonManager;

namespace Newskies.WebApi.Services
{
    public interface IUserSessionService
    {
        Task<LogonResponse> Logon(LogonRequestData logonRequestData);
        Task<bool> Logout();
        Task<string> GetAnonymousSharedSignature();
        Task ClearAnonymousSharedSignature();
        Task AnonymousLogonUnique();
        Task KeepAlive();
        Task<bool> IsAnonymousSession();
        Task<SessionInfo> GetSessionInfo();
        Task SetPassword(LogonRequestData logonRequestData, string newPassword);
    }

    public class UserSessionService : ServiceBase, IUserSessionService
    {
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _sharedSignatureCacheKey;
        private readonly IMemoryCache _cache;
        private readonly LogonRequestData _defLogonRequestData;
        private readonly IMapper _mapper;
        private readonly ISessionBagService _sessionBag;
        private readonly IAgentManager _agentClient;
        private readonly IPersonManager _personClient;
        private readonly nsk.ISessionManager _client;
        private readonly TimeSpan _newskiesIdleTimeout;

        public UserSessionService(IMemoryCache cache, ISessionBagService sessionBag, IAgentManager agentClient, IPersonManager personClient,
            nsk.ISessionManager client, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings) 
            : base(appSettings)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _agentClient = agentClient ?? throw new ArgumentNullException(nameof(agentClient));
            _personClient = personClient ?? throw new ArgumentNullException(nameof(personClient));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _defLogonRequestData = new LogonRequestData
            {
                DomainCode = _newskiesSettings.AgentDomain,
                AgentName = _newskiesSettings.AnonymousAgentName,
                Password = _newskiesSettings.AnonymousAgentPassword,
                RoleCode = _newskiesSettings.AnonymousAgentRole
            };
            var localIpAddress = Task.Run(async () => await httpContextAccessor.HttpContext.GetServerIP()).Result;
            _sharedSignatureCacheKey = "_sharedSignature_" + localIpAddress.ToString();
            _newskiesIdleTimeout = appSettings.Value.ApplicationSessionOptions != null ? 
                appSettings.Value.ApplicationSessionOptions.NewskiesIdleTimeout : 
                throw new ArgumentNullException(nameof(appSettings.Value.ApplicationSessionOptions));
        }

        public async Task<LogonResponse> Logon(LogonRequestData logonRequestData)
        {
            if (!string.IsNullOrEmpty(await _sessionBag.AgentName()))
                throw new ResponseErrorException(ResponseErrorCode.AlreadyLoggedIn,
                    string.Format("Already logged in as {0}", await _sessionBag.AgentName()));
            nsk.LogonResponse logonResponse;
            var previousSignature = await _sessionBag.Signature();
            try
            {
                logonResponse = await _client.LogonAsync(new nsk.LogonRequest
                {
                    logonRequestData = _mapper.Map<nsk.LogonRequestData>(logonRequestData)
                });
            }
            catch (System.ServiceModel.FaultException e)
            {
                if (e.Message.Contains("No agent found") || e.Message.Contains("was not authenticated") || e.Message.StartsWith("Unable to find best role for agent"))
                    throw new ResponseErrorException(ResponseErrorCode.InvalidLogin, "Invalid login details. ");

                //Handle scenario: System.ServiceModel.FaultException: 'The agent (WW2/test005@test.com) must reset their password.'
                if (e.Message.Contains("must reset their password"))
                {
                    return new LogonResponse { MustChangePassword = true };
                }

                throw e;
            }
            var newSignature = logonResponse.Signature;
            var booking = await _sessionBag.Booking();
            if (booking != null && string.IsNullOrEmpty(booking.RecordLocator) && !string.IsNullOrEmpty(previousSignature))
            {
                var result = await _client.TransferSessionAsync(new nsk.TransferSessionRequest
                {
                    ContractVersion = _newskiesSettings.ApiContractVersion,
                    tokenRequest = new nsk.TokenRequest
                    {
                        Token = previousSignature,
                        ChannelType = nsk.ChannelType.API,
                        SystemType = nsk.SystemType.WebServicesAPI
                    }
                });
                newSignature = result.TransferSessionResponseData.Signature;
            }
            await _sessionBag.SetSignature(newSignature);
            //await _sessionBag.SetSignature(logonResponse.Signature);
            await _sessionBag.SetAgentName(logonRequestData.AgentName);
            await _sessionBag.SetAgentPassword(logonRequestData.Password);
            var agentInfo = await GetAgentInfo(logonRequestData.AgentName, logonRequestData.DomainCode);
            await _sessionBag.SetRoleCode(agentInfo.Item1);
            await _sessionBag.SetOrganizationCode(agentInfo.Item2);
            await _sessionBag.SetAgentId(agentInfo.Item3);
            await _sessionBag.SetPersonId(agentInfo.Item4);
            await _sessionBag.SetCustomerNumber(agentInfo.Item5);
            return _mapper.Map<LogonResponse>(logonResponse);
        }

        public async Task<bool> Logout()
        {
            await _client.LogoutAsync(new nsk.LogoutRequest
            {
                Signature = await _sessionBag.Signature()
            });
            await _sessionBag.Clear(true);
            return _mapper.Map<bool>(true);
        }

        public async Task<string> GetAnonymousSharedSignature()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var cachedSig = _cache.Get<string>(_sharedSignatureCacheKey);
                if (!string.IsNullOrEmpty(cachedSig))
                    return cachedSig;
                var logonResp = await PerformAnonymousLogon();
                return _cache.Set(_sharedSignatureCacheKey, logonResp.Signature,
                    new MemoryCacheEntryOptions().SetSlidingExpiration(_newskiesIdleTimeout));
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task ClearAnonymousSharedSignature()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var cachedSig = _cache.Get<string>(_sharedSignatureCacheKey);
                if (!string.IsNullOrEmpty(cachedSig))
                    _cache.Remove(_sharedSignatureCacheKey);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task AnonymousLogonUnique()
        {
            if (!string.IsNullOrEmpty(await _sessionBag.Signature()))
            {
                // user is already logged in
                if (!await IsAnonymousSession())
                    throw new ResponseErrorException(ResponseErrorCode.InternalException,
                        string.Format("Already logged in as {0}", await _sessionBag.AgentName()));
                // user session has already logged in as anonymous
                throw new ResponseErrorException(
                    ResponseErrorCode.InternalException, "Already logged in as unique anonymous.");
            }
            var logonResp = await PerformAnonymousLogon();
            await _sessionBag.SetSignature(logonResp.Signature);
        }

        public async Task KeepAlive()
        {
            if (string.IsNullOrEmpty(await _sessionBag.Signature()))
                return;
            await _client.KeepAliveAsync(
                new nsk.KeepAliveRequest(_navApiContractVer, await _sessionBag.Signature()));
        }

        private async Task<LogonResponse> PerformAnonymousLogon()
        {
            var logonResponse = await _client.LogonAsync(new nsk.LogonRequest
            {
                logonRequestData = _mapper.Map<nsk.LogonRequestData>(_defLogonRequestData)
            });
            return _mapper.Map<LogonResponse>(logonResponse);
        }

        private async Task<Tuple<string,string, long, long, string>> GetAgentInfo(string agentName, string domainCode)
        {
            var findAgentReqData = new FindAgentRequestData
            {
                AgentName = new Contracts.SearchString
                {
                    SearchType = Contracts.Enumerations.SearchType.ExactMatch,
                    Value = agentName
                },
                DomainCode = domainCode
            };
            var findAgentResponse = await FindAgent(findAgentReqData);
            var agentId = findAgentResponse.FindAgentResponseData.FindAgentList[0].AgentID;
            var agent = await GetAgent(agentId);
            var person = await GetAgentPerson(agent.PersonID);
            return new Tuple<string, string, long, long, string>(agent.AgentRoles[0].RoleCode, agent.AgentIdentifier.OrganizationCode, agentId, agent.PersonID, person.CustomerNumber);
        }

        private async Task<Contracts.FindAgentsResponse> FindAgent(FindAgentRequestData findAgentRequestData)
        {
            var mappedRequest = Mapper.Map<nskam.FindAgentRequestData>(findAgentRequestData);
            var response = await _agentClient.FindAgentsAsync(new FindAgentsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                FindAgentRequestData = mappedRequest
            });
            return Mapper.Map<Contracts.FindAgentsResponse>(response);
        }

        private async Task<Agent> GetAgent(long agentId)
        {
            var response = await _agentClient.GetAgentAsync(new GetAgentRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                GetAgentReqData = new nskam.GetAgentRequestData
                {
                    AgentID = agentId,
                    GetAgentBy = GetAgentBy.AgentID,
                    GetDetails = true
                }
            });
            return Mapper.Map<Agent>(response.Agent);
        }

        private async Task<PersonManager.Person> GetAgentPerson(long personId)
        {
            var response = await _personClient.GetPersonAsync(new GetPersonRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                GetPersonRequestData = new GetPersonRequestData
                {
                    GetPersonBy = GetPersonBy.GetPersonByID,
                    GetPersonByPersonID = new GetPersonByPersonID
                    {
                        PersonID = personId
                    }
                }
            });
            return Mapper.Map<PersonManager.Person>(response.Person);
        }

        public async Task<bool> IsAnonymousSession()
        {
            return string.IsNullOrEmpty(await _sessionBag.AgentName());
        }

        public async Task<SessionInfo> GetSessionInfo()
        {
            return new SessionInfo {
                AgentName = await _sessionBag.AgentName(),
                RoleCode = await _sessionBag.RoleCode(),
                OrganizationCode = await _sessionBag.OrganizationCode()
            };
        }

        public async Task SetPassword(LogonRequestData logonRequestData, string newPassword)
        {
            try
            {
                await _client.ChangePasswordAsync(new nsk.ChangePasswordRequest
                {
                    logonRequestData = Mapper.Map<nsk.LogonRequestData>(logonRequestData),
                    ContractVersion = _newskiesSettings.ApiContractVersion,
                    newPassword = newPassword
                });
            }
            catch (System.ServiceModel.FaultException e)
            {
                //"FaultException: The agent (WW2/test004@test.com) was not authenticated."
                if (e.Message.Contains("The agent") && e.Message.Contains("was not authenticated"))
                    throw new ResponseErrorException(ResponseErrorCode.InvalidCurrentPassword, "Invalid current password. ");

                //"FaultException: The agent (WW2/michael@mycorp.com) tried to set the same password."
                if (e.Message.Contains("The agent") && e.Message.Contains("tried to set the same password"))
                    throw new ResponseErrorException(ResponseErrorCode.InvalidNewPassword, "Invalid new password. ");

                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}