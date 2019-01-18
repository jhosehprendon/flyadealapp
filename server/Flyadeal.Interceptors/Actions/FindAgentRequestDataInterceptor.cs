using Flyadeal.Interceptors.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class FindAgentRequestDataInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var requestData = request as FindAgentRequestData;
            if (requestData == null)
            {
                return await Task.FromResult(request);
            }
            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            var appSettings = context.HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            requestData.OrganizationCode = await sessionBag.OrganizationCode();
            requestData.DomainCode = appSettings.Value.NewskiesSettings.AgentDomain;
            if (requestData.AgentName == null)
            {
                requestData.AgentName = new SearchString
                {
                    Value = "",
                    SearchType = SearchType.StartsWith
                };
            }
            requestData.DomainCode = appSettings.Value.NewskiesSettings.AgentDomain;
            requestData.PageSize = Constants.PageSize;
            return await Task.FromResult(requestData);
        }
    }
}
