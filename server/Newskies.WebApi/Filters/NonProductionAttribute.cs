using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class NonProductionAttribute :ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var env = context.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            if (env != null && env.IsProduction())
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
