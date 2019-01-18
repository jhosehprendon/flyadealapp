using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Newskies.WebApi.Swagger
{
    internal static class SwaggerHelper
    {
        internal static void AddFlyadealSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            var docVersion = configuration.GetValue<string>("Swagger:DocVersion");
            var docName = configuration.GetValue<string>("Swagger:DocName");
            var docTitle = configuration.GetValue<string>("Swagger:DocTitle");
            if (string.IsNullOrEmpty(docVersion) || string.IsNullOrEmpty(docName) || string.IsNullOrEmpty(docTitle))
            {
                return;
            }
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AddSessionTokenHeaderParameter>();
                c.SwaggerDoc(docName, new Info { Title = docTitle, Version = docVersion });
            });
        }
    }
}