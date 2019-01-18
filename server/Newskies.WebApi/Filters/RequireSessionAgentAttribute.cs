using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Services;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class RequireSessionAgentAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService != null && !string.IsNullOrEmpty(await sessionBagService.AgentName()) && await sessionBagService.AgentId() > 0)
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }
            context.Result = new ResponseErrorException(ResponseErrorCode.NotLoggedIn,
                "Agent not logged in. ").ErrorActionResult();
        }
    }
}
