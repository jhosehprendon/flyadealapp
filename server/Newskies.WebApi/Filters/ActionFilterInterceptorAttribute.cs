using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Filters
{
    public class ActionFilterInterceptorAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var settings = context.HttpContext.RequestServices.GetService(typeof(IOptions<InterceptorsSettings>)) as IOptions<InterceptorsSettings>;
            var routeData = context.HttpContext.GetRouteData();
            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();

            var requestInterceptorTuples = settings.Value.Interceptors.SelectMany(i =>
            {
                var matched = (i.Controller == null || i.Controller.Equals(controllerName, StringComparison.Ordinal) || i.Controller == "*") && (i.Action == null || i.Action == "*" || i.Action.Equals(actionName, StringComparison.Ordinal));
                if (!matched || i.Request == null)
                {
                    return new Tuple<RequestInterceptor, object>[0];
                }
                return i.Request.Select(v =>
                {
                    var argumentPair = context.ActionArguments.Where(pair => pair.Value != null && pair.Key == v.ParameterName && v.Type.Replace(" ", "").Equals(pair.Value.GetTypeName()));
                    if (!argumentPair.Any())
                    {
                        return null;
                    }
                    return Tuple.Create(v, argumentPair.First().Value);
                }).Where(pair => pair != null);
            }).ToArray();

            if (!requestInterceptorTuples.Any())
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }
           
            var factory = context.HttpContext.RequestServices.GetService(typeof(IInterceptorFactory)) as IInterceptorFactory;
            try
            {
                foreach (var tuple in requestInterceptorTuples)
                {
                    var types = tuple.Item1.Interceptors;
                    foreach (var type in types)
                    {
                        var instance = factory.Create<IRequestInterceptor>(type);
                        var parameters = settings.Value.Parameters.ToList().Find(p => p.InterceptorType.Replace(" ", "").Equals(type.Replace(" ", "")));
                        await instance.OnRequest(tuple.Item2, context, parameters?.Settings);
                    }
                }
            }
            catch (Exception e)
            {
                context.Result = e.ErrorActionResult();
            }
            await base.OnActionExecutionAsync(context, next);
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var settings = context.HttpContext.RequestServices.GetService(typeof(IOptions<InterceptorsSettings>)) as IOptions<InterceptorsSettings>;
            var routeData = context.HttpContext.GetRouteData();
            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();
            var okResult = context.Result as OkObjectResult;
            if (okResult == null)
            {
                await base.OnResultExecutionAsync(context, next);
                return;
            }
            var value = okResult.Value;
            var responseInterceptors = settings.Value.Interceptors.SelectMany(i =>
            {
                // TODO: implement wildcard string search
                var matched = (i.Controller == null || i.Controller.Equals(controllerName, StringComparison.Ordinal) || i.Controller == "*") && (i.Action == null || i.Action == "*" || i.Action.Equals(actionName, StringComparison.Ordinal));
                if (!matched || i.Response == null)
                {
                    return new ResponseInterceptor[0];
                }
                return i.Response.Where(v => v.Type.Replace(" ", "").Equals(value.GetTypeName()) && v.Interceptors != null);
            }).ToArray();
            
            if (!responseInterceptors.Any())
            {
                await base.OnResultExecutionAsync(context, next);
                return;
            }
            var factory = context.HttpContext.RequestServices.GetService(typeof(IInterceptorFactory)) as IInterceptorFactory;
            try
            {
                foreach (var interceptor in responseInterceptors)
                {
                    var types = interceptor.Interceptors;
                    foreach (var type in types)
                    {
                        var instance = factory.Create<IResponseInterceptor>(type);
                        var parameters = settings.Value.Parameters.ToList().Find(p => p.InterceptorType.Replace(" ", "").Equals(type.Replace(" ", "")));
                        await instance.OnResponse(value, context, parameters?.Settings);
                    }
                }
            }
            catch (Exception e)
            {
                context.Result = e.ErrorActionResult();
            }
            await base.OnResultExecutionAsync(context, next);
        }
    }
}
