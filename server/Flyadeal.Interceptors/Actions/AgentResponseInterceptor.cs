using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class AgentResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var data = response as AgentResponseData;
            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (data == null || sessionBag == null)
                return await Task.FromResult(response);
            var agent = data.Agent;
            if (agent.AgentIdentifier.OrganizationCode != await sessionBag.OrganizationCode())
            {
                throw new ResponseErrorException(ResponseErrorCode.AgentUnauthorised,
                    string.Format("Not authorized to retrieve this account. "));
            }
            return data;
        }
    }
}
