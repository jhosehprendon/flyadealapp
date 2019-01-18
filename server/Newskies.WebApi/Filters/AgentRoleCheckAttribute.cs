using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Services;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class AgentRoleCheckAttribute : ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is OkResult)
            {
                var agentService = context.HttpContext.RequestServices.GetService(typeof(IAgentService)) as AgentService;
                var agent = await agentService.GetAgent();
                if (agent.Agent != null && agent.Agent.AgentRoles != null)
                {
                    var appSettings = context.HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                    if (appSettings != null && appSettings.Value != null && appSettings.Value.NewskiesSettings != null)
                    {
                        var nskSettings = appSettings.Value.NewskiesSettings;
                        var roles = agent.Agent.AgentRoles.ToList();
                        if (roles.Find(
                            r => r.RoleCode == nskSettings.DefaultMemberRoleCode || r.RoleCode == nskSettings.DefaultAgentRoleCode || 
                            r.RoleCode == nskSettings.MasterAgentRoleCode || r.RoleCode == nskSettings.MasterCorporateRoleCode ||
                            r.RoleCode == nskSettings.DefaultCorporateRoleCode) == null)
                        {
                            var userSessionService = context.HttpContext.RequestServices.GetService(typeof(IUserSessionService)) as UserSessionService;
                            await userSessionService.Logout();
                            context.Result = ResponseErrorExtensions.ReturnError(HttpStatusCode.BadRequest, 
                                new ResponseErrorException(Contracts.Enumerations.ResponseErrorCode.AgentUnauthorised, "Agent Logon not allowed. "));
                        }
                    }
                }
            }
            await base.OnResultExecutionAsync(context, next);
        }
    }
}
