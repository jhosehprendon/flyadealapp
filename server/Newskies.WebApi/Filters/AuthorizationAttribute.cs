using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService != null && (await sessionBagService.Initialised())) {
                await next();
                return;
            }
            context.Result = new UnauthorizedResult();
        }
    }
}
