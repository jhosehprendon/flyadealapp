using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Newskies.WebApi.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errMsgs = new List<string>();
                foreach (var item in context.ModelState)
                {
                    foreach (var item2 in item.Value.Errors)
                    {
                        if (item2.Exception != null)
                        {
                            context.Result = new ResponseErrorException(
                                ResponseErrorCode.InvalidRequest, item2.Exception.Message).ErrorActionResult();
                            return;
                        }
                        errMsgs.Add(item2.ErrorMessage);
                    }
                }
                context.Result = new ResponseErrorException(ResponseErrorCode.InvalidRequest, 
                    errMsgs.ToArray()).ErrorActionResult();
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
