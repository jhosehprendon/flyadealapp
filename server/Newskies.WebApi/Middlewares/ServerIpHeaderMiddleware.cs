using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Newskies.WebApi.Extensions;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;

namespace Newskies.WebApi.Middlewares
{
    public class ServerIdHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public ServerIdHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IOptions<AppSettings> appSettings)
        {
            if (appSettings != null && appSettings.Value != null && appSettings.Value.IncludeServerIdHeader)
            {
                var ipStr = await context.GetServerIP();
                if (!string.IsNullOrEmpty(ipStr))
                {
                    var split = ipStr.Split('.');
                    var idStr = split.Length > 0 ? split[split.Length - 1] : ipStr;
                    context.Response.Headers.Add("sid", new StringValues(idStr));
                }
            }
            await _next(context);
        }
    }
}
