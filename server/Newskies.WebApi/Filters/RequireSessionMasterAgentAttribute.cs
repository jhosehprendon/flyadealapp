using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Services;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class RequireSessionMasterAgentAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            var appSettings = context.HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            var sessionRoleCode = await sessionBagService.RoleCode();
            if (sessionBagService != null && !string.IsNullOrEmpty(await sessionBagService.AgentName()) && await sessionBagService.AgentId() > 0 
                && sessionRoleCode == appSettings.Value.NewskiesSettings.MasterAgentRoleCode || sessionRoleCode == appSettings.Value.NewskiesSettings.MasterCorporateRoleCode)
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }
            context.Result = new ResponseErrorException(ResponseErrorCode.AgentUnauthorised,
                "Agent not authorised to perform action. ").ErrorActionResult();
        }
    }
}
