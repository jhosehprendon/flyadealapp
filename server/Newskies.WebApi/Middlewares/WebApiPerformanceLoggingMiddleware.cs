using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;
using System.Diagnostics;

namespace Newskies.WebApi.Middlewares
{
    public class WebApiPerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebApiPerformanceLoggingMiddleware> _logger;

        public WebApiPerformanceLoggingMiddleware(RequestDelegate next, ILogger<WebApiPerformanceLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("/api/"))
            {
                var stopWatch = Stopwatch.StartNew();
                context.Response.OnCompleted(CompleteLog, stopWatch);
            }
            await _next(context);
        }

        private Task CompleteLog(object arg)
        {
            var stopwatch = arg as Stopwatch;
            if (stopwatch != null)
            {
                stopwatch.Stop();
                _logger.WriteTimedLog(stopwatch.ElapsedMilliseconds);
            }
            return Task.FromResult(0);
        }
    }
}
