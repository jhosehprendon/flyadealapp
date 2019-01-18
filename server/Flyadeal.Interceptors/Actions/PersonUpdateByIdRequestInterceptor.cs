using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using Navitaire.WebServices.DataContracts.Common;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.AgentManager;
using Navitaire.WebServices.DataContracts.Common.Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.WebApi.Contracts.Enumerations;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class PersonUpdateByIdRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var personId = (int)request;
            if (personId == 0)
                return await Task.FromResult(request);
            var httpContext = context.HttpContext;
            var sessionBagService = httpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null)
                return await Task.FromResult(request);
            var agentManager = httpContext.RequestServices.GetService(typeof(IAgentManager)) as IAgentManager;
            var appSettings = httpContext.RequestServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            var nskSettings = appSettings.Value.NewskiesSettings;
            var getAgentResponse = await agentManager.GetAgentAsync(new GetAgentRequest
            {
                ContractVersion = nskSettings.ApiContractVersion,
                MessageContractVersion = nskSettings.MsgContractVersion,
                Signature = await sessionBagService.Signature(),
                EnableExceptionStackTrace = false,
                GetAgentReqData = new GetAgentRequestData
                {
                    PersonID = personId,
                    GetAgentBy = GetAgentBy.PersonID
                    //GetDetails = true
                }
            });
            if (getAgentResponse == null || getAgentResponse.Agent == null)
            {
                throw new ResponseErrorException(ResponseErrorCode.PersonUpdateUnauthorised, "Unauthorised person update. ");
            }
            if (getAgentResponse.Agent.AgentIdentifier.OrganizationCode != await sessionBagService.OrganizationCode())
            {
                throw new ResponseErrorException(ResponseErrorCode.PersonUpdateUnauthorised, "Unauthorised person update. ");
            }
            return await Task.FromResult(request);
        }
    }
}
