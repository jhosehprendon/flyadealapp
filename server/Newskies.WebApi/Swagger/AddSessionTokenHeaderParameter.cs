using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System;

namespace Newskies.WebApi.Swagger
{
    public class AddSessionTokenHeaderParameter : IOperationFilter
    {
        private ApplicationSessionOptions _options;

        public AddSessionTokenHeaderParameter(IOptions<AppSettings> appSettings)
        {
            _options = appSettings.Value != null && appSettings.Value.ApplicationSessionOptions != null ?
                appSettings.Value.ApplicationSessionOptions : 
                throw new ArgumentNullException(nameof(appSettings.Value.ApplicationSessionOptions));
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = _options.SessionTokenHeader,
                In = "header",
                Type = "string",
                Required = false
            });
        }
    }
}