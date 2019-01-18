using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    /// <summary>
    /// Need this extension when app is running in a virtual directory and URL
    /// without trailing slash, which breaks asset links in index.html
    /// </summary>
    public class RequireTrailingSlashAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null || context.HttpContext == null || context.HttpContext.Request == null)
            {
                await next();
                return;
            }
            var displayUrl = context.HttpContext.Request.GetDisplayUrl();
            if (!displayUrl.EndsWith("/") && context.HttpContext.Request.Method == "GET")
            {
                context.Result = new RedirectResult(displayUrl + "/");
                return;
            }
            await next();
        }
    }
}
