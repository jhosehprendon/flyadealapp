using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Services;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            Booking booking = null;
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as ISessionBagService;
            if (sessionBagService != null)
            {
                try
                {
                    booking = await sessionBagService.Booking();
                }
                catch { }
            }
            var cad = context.ActionDescriptor as ControllerActionDescriptor;
            var controller = cad != null ? cad.ControllerName : "";
            var action = cad != null ? cad.ActionName : "";
            context.Result = context.Exception.ErrorActionResult(controller, action, booking != null ? booking.RecordLocator : "");

            // Handles case where Navitaire API session token expires or corrupts due to their server restart or other reasons
            if (context.Exception is FaultException && context.Exception.Message.ToLower().Contains("session token authentication failure"))
            {
                if (string.IsNullOrEmpty(await sessionBagService.Signature()))
                {
                    var userSessionService = context.HttpContext.RequestServices.GetService(typeof(IUserSessionService)) as UserSessionService;
                    await userSessionService.ClearAnonymousSharedSignature();
                }
                await sessionBagService.Clear();
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
