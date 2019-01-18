using AutoMapper;
using Microsoft.Extensions.Options;
using Navitaire.WebServices.DataContracts.Common;
using nskCommonEnum = Navitaire.WebServices.DataContracts.Common.Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.AgentManager;
using Newskies.PersonManager;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;
using System.Collections.Generic;

namespace Newskies.WebApi.Services
{
    public interface IAgentService
    {
        Task<dto.FindAgentsResponse> FindAgent(dto.FindAgentRequestData findAgentRequestData);
        Task<dto.AgentResponseData> GetAgent(long id = 0);
        Task<dto.CommitAgentResponse> AddAgent(dto.CommitAgentRequestData commitAgentRequestData);
        Task<dto.Agent> UpdateAgent(dto.Agent agent);
        Task<dto.Person> UpdatePerson(dto.Person person);
        Task<string> PasswordReset(dto.PasswordResetRequest passwordResetRequest);
        Task PasswordSet(dto.PasswordSetRequest passwordSetRequest);
        Task<dto.FindAgentsResponse> GetAgentList(dto.FindAgentRequestData findAgentRequestData = null);
    }

    public class AgentService : ServiceBase, IAgentService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IAgentManager _client;
        private readonly IPersonManager _personClient;
        private readonly IUserSessionService _userSessionService;

        public AgentService(ISessionBagService sessionBag, IAgentManager client, IPersonManager personClient, IUserSessionService userSessionService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _personClient = personClient ?? throw new ArgumentNullException(nameof(personClient));
        }

        public async Task<dto.FindAgentsResponse> FindAgent(dto.FindAgentRequestData findAgentRequestData)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            var mappedRequest = Mapper.Map<FindAgentRequestData>(findAgentRequestData);
            var response = await _client.FindAgentsAsync(new FindAgentsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                FindAgentRequestData = mappedRequest
            });
            return Mapper.Map<dto.FindAgentsResponse>(response);
        }

        public async Task<dto.AgentResponseData> GetAgent(long id = 0)
        {
            var agent = await GetNskAgent(id > 0 ? id : await _sessionBag.AgentId());
            if (agent == null)
            {
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AgentNotFound,
                    string.Format("Account not found. "));
            }
            var agentPerson = await GetPerson(agent.PersonID);
            var agentResponseData = new dto.AgentResponseData
            {
                Person = Mapper.Map<dto.Person>(agentPerson),
                Agent = Mapper.Map<dto.Agent>(agent)
            };
            return agentResponseData;
        }

        public async Task<dto.CommitAgentResponse> AddAgent(dto.CommitAgentRequestData commitAgentRequestData)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();

            // ensure agent does not already exist
            dto.FindAgentsResponse findAgentResponse = null;
            try
            {
                findAgentResponse = await FindAgent(new dto.FindAgentRequestData
                {
                    AgentName = new dto.SearchString
                    {
                        SearchType = dto.Enumerations.SearchType.ExactMatch,
                        Value = commitAgentRequestData.Agent.LoginName.ToLower()
                    },
                    DomainCode = _newskiesSettings.AgentDomain
                });
            }
            catch { }
            if (findAgentResponse != null && findAgentResponse.FindAgentResponseData != null &&
                findAgentResponse.FindAgentResponseData.FindAgentList != null &&
                findAgentResponse.FindAgentResponseData.FindAgentList.Length != 0)
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AgentAlreadyExists,
                    string.Format("Account '{0}' already exists. ", commitAgentRequestData.Agent.LoginName.ToLower()));

            var mappedRequest = Mapper.Map<CommitAgentRequestData>(commitAgentRequestData);
            SetAgentProperties(mappedRequest);
            var commitAgentResponse = await CommitAgent(mappedRequest, signature);
            return Mapper.Map<dto.CommitAgentResponse>(commitAgentResponse);
        }

        public async Task<dto.Agent> UpdateAgent(dto.Agent agent)
        {
            // Disallow self to update status/role/lock
            if (agent.AgentID == await _sessionBag.AgentId())
            {
                var self = await GetAgent(agent.AgentID);
                if (IsDifferentRolesOrStatusOrLocked(agent, self.Agent))
                {
                    throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AgentUpdateNotAllowed, "Agent update operation not allowed. ");
                }
            }

            var signature = await _sessionBag.Signature();
            var mappedAgent = Mapper.Map<Agent>(agent);
            mappedAgent.AgentIdentifier = new AgentIdentifier
            {
                AgentName = agent.LoginName,
                OrganizationCode = await _sessionBag.OrganizationCode(),
                State = nskCommonEnum.MessageState.Clean
            };
            var request = new CommitAgentRequestData { Agent = mappedAgent };
            SetAgentProperties(request);
            request.Agent.State = nskCommonEnum.MessageState.Modified;
            request.Agent.AgentRoles = await SetAgentRoleCode(agent.AgentID, request.Agent.AgentRoles);
            await CommitAgent(request, signature);
            var getAgent = await GetAgent(agent.AgentID);
            return getAgent.Agent;
        }

        public async Task<dto.Person> UpdatePerson(dto.Person person)
        {
            var nskPerson = await MergePerson(person);
            var request = new CommitPersonRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                personReqData = nskPerson
            };
            var commitResponse = await _personClient.CommitAsync(request);
            return Mapper.Map<dto.Person>(commitResponse.Person);
        }

        public async Task PasswordSet(dto.PasswordSetRequest passwordSetRequest)
        {
            var logonRequestData = new dto.LogonRequestData
            {
                AgentName = await _sessionBag.AgentName(),
                DomainCode = _newskiesSettings.AgentDomain,
                RoleCode = await _sessionBag.RoleCode(),
                Password = await _sessionBag.AgentPassword()
            };
            await _userSessionService.SetPassword(logonRequestData, passwordSetRequest.NewPassword);
            await _sessionBag.SetAgentPassword(passwordSetRequest.NewPassword);
        }

        public async Task<string> PasswordReset(dto.PasswordResetRequest passwordResetRequest)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            dto.FindAgentsResponse findAgentResponse;
            try
            {
                findAgentResponse = await FindAgent(new dto.FindAgentRequestData
                {
                    AgentName = new dto.SearchString
                    {
                        SearchType = dto.Enumerations.SearchType.ExactMatch,
                        Value = passwordResetRequest.LoginName
                    },
                    DomainCode = _newskiesSettings.AgentDomain,
                    Status = dto.Enumerations.AgentStatus.Active
                });
            }
            catch (System.ServiceModel.FaultException e)
            {
                if (e.Message.StartsWith("No agent found"))
                    throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.InvalidLogin, "Invalid login details. ");
                throw e;
            }
            if (findAgentResponse == null || findAgentResponse.FindAgentResponseData == null ||
                findAgentResponse.FindAgentResponseData.FindAgentList == null ||
                findAgentResponse.FindAgentResponseData.FindAgentList.Length == 0)
            {
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AgentNotFound, string.Format("Account '{0}' not found. ", passwordResetRequest.LoginName));
            }
            var foundAgent = findAgentResponse.FindAgentResponseData.FindAgentList[0];
            /** Password Reset will be called in the SQS/Droid workaround solution
             *
            var newPassword = !string.IsNullOrEmpty(passwordResetRequest.NewPassword) ? passwordResetRequest.NewPassword : RandomPassword();
            await _client.ResetPasswordAsync(new ResetPasswordRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                ResetPasswordReqData = new ResetPasswordRequestData
                {
                    AgentID = foundAgent.AgentID,
                    NewPassword = newPassword
                }
            });
            ***/
            return string.Format("{0}\\{1}", _newskiesSettings.AgentDomain, passwordResetRequest.LoginName);
        }

        public async Task<dto.FindAgentsResponse> GetAgentList(dto.FindAgentRequestData findAgentRequestData = null)
        {
            var signature = await _sessionBag.Signature();
            var requestData = findAgentRequestData != null ? Mapper.Map<FindAgentRequestData>(findAgentRequestData)
                : new FindAgentRequestData
                {
                    DomainCode = _newskiesSettings.AgentDomain,
                    OrganizationCode = await _sessionBag.OrganizationCode(),
                    AgentName = new Navitaire.WebServices.DataContracts.Common.ParentMessageBase.SearchString
                    {
                        Value = "",
                        SearchType = nskCommonEnum.SearchType.StartsWith
                    },
                    PageSize = 10
                };
            var response = await _client.FindAgentsAsync(new FindAgentsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                FindAgentRequestData = requestData
            });
            var mappedResponse = Mapper.Map<dto.FindAgentsResponse>(response);
            return mappedResponse;
        }

        private async Task<Agent> GetNskAgent(long agentId)
        {
            var getAgentResponse = await _client.GetAgentAsync(new GetAgentRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                GetAgentReqData = new GetAgentRequestData
                {
                    AgentID = agentId,
                    GetAgentBy = nskCommonEnum.GetAgentBy.AgentID,
                    GetDetails = true
                }
            });
            return getAgentResponse.Agent;
        }

        private async Task<Person> GetPerson(long personID)
        {
            var getPersonResponse = await _personClient.GetPersonAsync(new GetPersonRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                GetPersonRequestData = new GetPersonRequestData
                {
                    GetDetails = true,
                    GetPersonBy = GetPersonBy.GetPersonByID,
                    GetPersonByPersonID = new GetPersonByPersonID
                    {
                        PersonID = personID
                    }
                }
            });
            return getPersonResponse.Person;
        }

        private async Task<Person> MergePerson(dto.Person person)
        {
            var personId = person.PersonID > 0 ? person.PersonID : await _sessionBag.PersonId();
            var nskPerson = await GetPerson(personId);
            var changedPerson = Mapper.Map<Person>(person);
            nskPerson.State = MessageState.Modified;
            nskPerson.DOB = changedPerson.DOB;
            nskPerson.CultureCode = changedPerson.CultureCode;
            nskPerson.Gender = changedPerson.Gender;
            nskPerson.Nationality = changedPerson.Nationality;
            nskPerson.ResidentCountry = changedPerson.ResidentCountry;
            if (changedPerson.TravelDocs != null && changedPerson.TravelDocs.Length > 0)
            {
                var newTravelDocs = changedPerson.TravelDocs.ToList();
                newTravelDocs[0].Default = true;
                newTravelDocs.ForEach(p => { p.PersonID = personId; p.State = MessageState.New; });
                var updatedNskDocs = nskPerson.TravelDocs != null ? nskPerson.TravelDocs.ToList() : new List<TravelDoc>();
                updatedNskDocs.ForEach(p => { p.State = MessageState.Deleted; });
                updatedNskDocs.AddRange(newTravelDocs);
                nskPerson.TravelDocs = updatedNskDocs.ToArray();
            }
            if (changedPerson.PersonNameList != null)
            {
                var newPersonNames = changedPerson.PersonNameList.ToList();
                newPersonNames[0].NameType = NameType.True;
                newPersonNames.ForEach(p => { p.PersonID = personId; p.State = MessageState.New; });
                var updatedPersonNames = nskPerson.PersonNameList != null ? nskPerson.PersonNameList.ToList() : new List<PersonName>();
                updatedPersonNames.ForEach(p => { p.State = MessageState.Deleted; });
                updatedPersonNames.AddRange(newPersonNames);
                nskPerson.PersonNameList = updatedPersonNames.ToArray();
            }
            if (changedPerson.PersonPhoneList != null)
            {
                var newPersonPhones = changedPerson.PersonPhoneList.ToList();
                newPersonPhones[0].Default = true;
                newPersonPhones.ForEach(p => { p.PersonID = personId; p.State = MessageState.New; });
                var updatedPersonPhones = nskPerson.PersonPhoneList != null ? nskPerson.PersonPhoneList.ToList() : new List<PersonPhone>();
                updatedPersonPhones.ForEach(p => { p.State = MessageState.Deleted; });
                updatedPersonPhones.AddRange(newPersonPhones);
                nskPerson.PersonPhoneList = updatedPersonPhones.ToArray();
            }
            if (changedPerson.PersonAddressList != null)
            {
                var newPersonAddresses = changedPerson.PersonAddressList.ToList();
                newPersonAddresses[0].Default = true;
                newPersonAddresses.ForEach(p => { p.PersonID = personId; p.State = MessageState.New; });
                var updatedPersonAddresses = nskPerson.PersonAddressList != null ? nskPerson.PersonAddressList.ToList() : new List<PersonAddress>();
                updatedPersonAddresses.ForEach(p => { p.State = MessageState.Deleted; });
                updatedPersonAddresses.AddRange(newPersonAddresses);
                nskPerson.PersonAddressList = updatedPersonAddresses.ToArray();
            }
            return nskPerson;
        }

        private async Task<CommitAgentResponse> CommitAgent(CommitAgentRequestData commitAgentRequestData, string signature)
        {
            return await _client.CommitAgentAsync(new CommitAgentRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                CommitAgentReqData = commitAgentRequestData
            });
        }

        private void SetAgentProperties(CommitAgentRequestData request, bool forcePasswordReset = false)
        {
            request.Agent.AuthenticationType = nskCommonEnum.AuthenticationType.Password;
            request.Agent.ForcePasswordReset = forcePasswordReset;
            if (request.Agent.AgentRoles == null || request.Agent.AgentRoles.Length == 0)
            {
                request.Agent.AgentRoles = GetNewRole(_newskiesSettings.DefaultAgentRoleCode);
            }
            if (string.IsNullOrEmpty(request.Agent.DepartmentCode))
            {
                request.Agent.DepartmentCode = _newskiesSettings.DefaultAgentDepartmentCode;
            }
            if (string.IsNullOrEmpty(request.Agent.LocationCode))
            {
                request.Agent.LocationCode = _newskiesSettings.DefaultAgentLocationCode;
            }
            if (string.IsNullOrEmpty(request.Agent.AgentIdentifier.DomainCode))
            {
                request.Agent.AgentIdentifier.DomainCode = _newskiesSettings.AgentDomain;
            }
            if (string.IsNullOrEmpty(request.Agent.AgentIdentifier.OrganizationCode))
            {
                request.Agent.AgentIdentifier.OrganizationCode = _newskiesSettings.DefaultAgentOrgCode;
            }
            if (request.Person != null && string.IsNullOrEmpty(request.Person.CultureCode))
            {
                request.Person.CultureCode = _newskiesSettings.DefaultCulture;
            }
        }

        private async Task<AgentRole[]> SetAgentRoleCode(long agentId, AgentRole[] requestedAgentRoles)
        {
            if (requestedAgentRoles == null || requestedAgentRoles.Length == 0)
            {
                return null;
            }
            var getAgentResp = await _client.GetAgentAsync(new GetAgentRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                GetAgentReqData = new GetAgentRequestData
                {
                    GetAgentBy = nskCommonEnum.GetAgentBy.AgentID,
                    AgentID = agentId,
                    GetDetails = true
                }
            });
            var requestedAgentRoleCode = requestedAgentRoles[0].RoleCode;
            if (getAgentResp.Agent.AgentRoles.ToList().Exists(p => p.RoleCode == requestedAgentRoleCode))
            {
                // no role change
                return null;
            }
            var updatedRoles = new AgentRole[getAgentResp.Agent.AgentRoles.Length + 1];
            // delete currently assigned roles
            for (var i = 0; i < getAgentResp.Agent.AgentRoles.Length; i++)
            {
                var role = getAgentResp.Agent.AgentRoles[i];
                role.State = nskCommonEnum.MessageState.Deleted;
                updatedRoles[i] = role;
            }
            // assign new role requested
            updatedRoles[updatedRoles.Length - 1] = GetNewRole(requestedAgentRoleCode)[0];
            return updatedRoles;
        }

        private AgentRole[] GetNewRole(string roleCode)
        {
            return new AgentRole[] { new AgentRole
                {
                    RoleCode = roleCode,
                    EffectiveDate = DateTime.UtcNow.AddDays(-1),
                    EndEffectiveDate = NewskiesHelper.DATE_TIME_MAX_VALUE,
                    EffectiveDOW = nskCommonEnum.DOW.Daily
                }};
        }

        private string RandomPassword(int passwordLength = 12)
        {
            string lcChaars = "abcdefghijkmnopqrstuvwxyz";
            string ucChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            string numChars = "0123456789";
            string symChars = "!@#$%^&*()";
            string allAllowedChars = lcChaars + ucChars + numChars + symChars;
            var chars = new char[passwordLength];
            var rd = new Random();
            for (int i = 0; i < passwordLength - 4; i++)
            {
                chars[i] = allAllowedChars[rd.Next(0, allAllowedChars.Length)];
            }
            chars[passwordLength - 4] = numChars[rd.Next(0, numChars.Length)];
            chars[passwordLength - 3] = lcChaars[rd.Next(0, lcChaars.Length)];
            chars[passwordLength - 2] = ucChars[rd.Next(0, ucChars.Length)];
            chars[passwordLength - 1] = symChars[rd.Next(0, symChars.Length)];
            return new string(chars);
        }

        private bool IsDifferentRolesOrStatusOrLocked(dto.Agent self, dto.Agent requestAgent)
        {
            if (self.Status != requestAgent.Status || requestAgent.Locked != self.Locked)
            {
                return true;
            }
            if ((self.AgentRoles == null || self.AgentRoles.Length == 0) && (requestAgent.AgentRoles == null || requestAgent.AgentRoles.Length == 0))
            {
                return false;
            }
            var sorted1 = self.AgentRoles.OrderBy(o => o.RoleCode).Select(s => s.RoleCode);
            var sorted2 = requestAgent.AgentRoles.OrderBy(o => o.RoleCode).Select(s => s.RoleCode);
            return !sorted1.SequenceEqual(sorted2);
        }
    }
}
