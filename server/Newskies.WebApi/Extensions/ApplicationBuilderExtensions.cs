using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Middlewares;
using Newskies.WebApi.RewriteRules;
using System;
using System.IO;

namespace Newskies.WebApi.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IWebHostBuilder UseKestrelServer(this IWebHostBuilder builder, IConfiguration config)
        {
            return builder.UseKestrel(opt =>
            {
                opt.AddServerHeader = false;
                opt.Limits.KeepAliveTimeout = (TimeSpan)config.GetValue(typeof(TimeSpan), "ServerLimitsSettings:KeepAliveTimeout", opt.Limits.KeepAliveTimeout);
                opt.Limits.MaxRequestHeaderCount = (int)config.GetValue(typeof(int), "ServerLimitsSettings:MaxRequestHeaderCount", opt.Limits.MaxRequestHeaderCount);
                opt.Limits.MaxRequestHeadersTotalSize = (int)config.GetValue(typeof(int), "ServerLimitsSettings:MaxRequestHeadersTotalSizeBytes", opt.Limits.MaxRequestHeadersTotalSize);
                opt.Limits.MaxRequestLineSize = (int)config.GetValue(typeof(int), "ServerLimitsSettings:MaxRequestLineSizeBytes", opt.Limits.MaxRequestLineSize);
                opt.Limits.RequestHeadersTimeout = (TimeSpan)config.GetValue(typeof(TimeSpan), "ServerLimitsSettings:RequestHeadersTimeout", opt.Limits.RequestHeadersTimeout);
                opt.Limits.MaxRequestBufferSize = (long?)config.GetValue(typeof(long?), "ServerLimitsSettings:MaxRequestBufferSizeBytes", opt.Limits.MaxRequestBufferSize);
                opt.Limits.MaxResponseBufferSize = (long?)config.GetValue(typeof(long?), "ServerLimitsSettings:MaxResponseBufferSizeBytes", opt.Limits.MaxResponseBufferSize);
            });
        }

        /// <summary>
        /// Build API process pipeline.
        /// </summary>
        /// <param name="app"></param>
        internal static void UseApiRouter(this IApplicationBuilder app)
        {
            app.MapWhen(context =>
            {
                var apiRoute = context.Request.Path.Value.Contains("/api/");
                return apiRoute;
            }, builder =>
            {
                var options = app.ApplicationServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                if (options != null && options.Value != null && options.Value.CrossOriginRequests)
                {
                   builder.UseCors(corsBuilder =>
                   {
                       corsBuilder.WithExposedHeaders(options.Value.ApplicationSessionOptions.SessionTokenHeader);
                       corsBuilder.AllowAnyHeader();
                       corsBuilder.AllowAnyOrigin();
                       corsBuilder.AllowAnyMethod();
                   });
                }
                builder.UseMiddleware<SessionTokenHeaderMiddleware>();
                builder.UseMiddleware<ServerIdHeaderMiddleware>();
                builder.UseSession();
                builder.UseMvc();
            });
        }

        internal static void UseClientAppRouter(this IApplicationBuilder app)
        {
            app.UseIndexHtmlRouter();
            app.UseThreeDSecureRouter();
            app.UseClientStaticsRouter();
        }

        internal static void UseSwaggerRouter(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService(typeof(IOptions<SwaggerSettings>)) as IOptions<SwaggerSettings>;
            if (options == null || options.Value == null || string.IsNullOrEmpty(options.Value.Url))
                return;
            app.MapWhen(context =>
            {
                var path = context.Request.Path.Value;
                return path.Contains("/swagger/");
            }, builder =>
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(c => { c.SwaggerEndpoint(options.Value.Url, options.Value.Description); });
            });
        }

        internal static void UseWebApiPerformanceLogging(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            if (options != null && options.Value != null && options.Value.PerformanceLoggingSettings != null 
                && options.Value.PerformanceLoggingSettings.EnableWebApiLogging)
                app.UseMiddleware<WebApiPerformanceLoggingMiddleware>();
        }

        internal static void UseHttpsEnforcement(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            if (options != null && options.Value != null && options.Value.EnforceSSL)
                app.UseRewriter(new RewriteOptions().Add(new RewriteHttpsRule()));
        }

        internal static void UseCompressionResponse(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
            if (options != null && options.Value != null && options.Value.ResponseCompression)
            {
                app.UseResponseCompression();
            }
        }

        /// <summary>
        /// Just a route called by AWS ELB monitor.
        /// </summary>
        /// <param name="app"></param>
        internal static void UseReadMeRouter(this IApplicationBuilder app)
        {
            app.MapWhen(context =>
            {
                return context.Request.Path.Value.EndsWith("/readme.txt");
            }, builder =>
            {
                builder.Run(async (context) => {
                    var msg = System.Text.Encoding.Unicode.GetBytes("Hi ELB");
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";
                    await context.Response.Body.WriteAsync(msg, 0, msg.Length);
                });
            });
        }

        /// <summary>
        /// Creates route for index.html main client application file. Need to provide a special route
        /// because react-router is used in the client app, therefore need to load index.html for all react-router routes 
        /// (for example, /booking/search, /booking/search ), except /api/, /swagger/ or any of the static assets (images, fonts and styles)
        /// used in client application. index.html is loaded using HomeController Spa action
        /// </summary>
        /// <param name="app"></param>
        private static void UseIndexHtmlRouter(this IApplicationBuilder app)
        {
            app.MapWhen(context =>
            {
                var path = context.Request.Path.Value;
                return !path.Contains("/api/") && !path.Contains("/swagger/") && !path.Contains("/Tds/")
                                && !path.Contains(".css") && !path.Contains(".js")
                                && !path.Contains(".gif") && !path.Contains(".eot")
                                && !path.Contains(".woff") && !path.Contains(".woff2")
                                && !path.Contains(".ttf") && !path.Contains(".svg")
                                && !path.Contains(".jpg") && !path.Contains(".png")
                                && !path.Contains(".ico");
            }, builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapRoute("Spa", "{*url}", defaults: new { controller = "Home", action = "Spa" });
                });
            });
        }

        private static void UseThreeDSecureRouter(this IApplicationBuilder app)
        {
            app.MapWhen(context =>
            {
                var path = context.Request.Path.Value;
                return path.Contains("/Tds/");
            }, builder =>
            {
                builder.UseMvc();
            });
        }

        private static void UseClientStaticsRouter(this IApplicationBuilder app)
        {
            var clientStaticsPath = Path.Combine(ApplicationEnvironment.ApplicationBasePath, "static");
            if (!Directory.Exists(clientStaticsPath))
            {
                return;
            }
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(clientStaticsPath),
                EnableDirectoryBrowsing = false
            });
        }
    }
}
