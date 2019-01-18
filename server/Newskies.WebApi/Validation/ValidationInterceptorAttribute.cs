using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Newskies.WebApi.Validation
{
    public interface IValidationInterceptor
    {
        ValidationResult IsValid(object value, ValidationContext validationContext);
    }

    public class ValidationInterceptorAttribute : ValidationAttribute
    {
        public ValidationInterceptorAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInterceptors = FindPropertyInterceptors(validationContext);
           
            var typeNames = propertyInterceptors.SelectMany(i=> i.Interceptors);
            var factory = validationContext.GetService(typeof(IInterceptorFactory)) as IInterceptorFactory;
            var interceptors = typeNames.Select(typeName => factory.Create<IValidationInterceptor>(typeName)).ToArray();
            var results = interceptors.Select(i => i.IsValid(value, validationContext));
            var firstFailed = results.FirstOrDefault(r => r != ValidationResult.Success);
            return firstFailed ?? ValidationResult.Success;
        }

        private PropertyInterceptor[] FindPropertyInterceptors(ValidationContext validationContext)
        {
            var settings = validationContext.GetService(typeof(IOptions<InterceptorsSettings>)) as IOptions<InterceptorsSettings>;

            var contextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var routeData = contextAccessor.HttpContext.GetRouteData();
            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();

            var typeInfo = validationContext.ObjectType.GetTypeInfo();
            var assemblyName = typeInfo.Assembly.GetName().Name;
            var fullTypeName = typeInfo.FullName;


            var result = settings.Value.Interceptors.SelectMany(i =>
            {
                // TODO: NotImplementedException wildcard string search
                var matched = (i.Controller == null || i.Controller.Equals(controllerName, StringComparison.Ordinal) || i.Controller == "*") && (i.Action == null || i.Action == "*" || i.Action.Equals(actionName, StringComparison.Ordinal));
                if (!matched || i.Validation == null)
                {
                    return new PropertyInterceptor[0];
                }
                return i.Validation.Where(v => v.Type.Replace(" ", "").Equals($"{fullTypeName},{assemblyName}") && v.Property.Equals(validationContext.MemberName));
            }).ToArray();
            return result;
        }
    }
}
