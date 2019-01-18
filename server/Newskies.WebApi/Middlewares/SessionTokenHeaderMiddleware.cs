using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;
using System;
using Newskies.WebApi.Configuration;

namespace Newskies.WebApi.Middlewares
{
    public class SessionTokenHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionTokenHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IOptions<AppSettings> appSettings)
        {
            var options = appSettings.Value != null && appSettings.Value.ApplicationSessionOptions != null ?
                appSettings.Value.ApplicationSessionOptions : 
                throw new ArgumentNullException(nameof(appSettings.Value.ApplicationSessionOptions));
            context.Response.OnStarting(OnStartingCallback, Tuple.Create(context, options));
            StringValues sessionToken;
            var sessionTokenPresented = context.Request.Headers.TryGetValue(options.SessionTokenHeader, out sessionToken);
            if (sessionTokenPresented && sessionToken.Any())
            {
                var sessionCookieName = options.Cookie.Name;
                // OPTION 1 - delete "Cookie" header completely and add only session cookie
                context.Request.Headers.Remove("Cookie");
                context.Request.Headers.Add("Cookie",
                    new StringValues($"{sessionCookieName}={sessionToken.First()}"));

                // OPTION 2 - append "x-session-token" header value to the request cookie collection
                //StringValues cookieValues;
                //if(!context.Request.Headers.TryGetValue("Cookie", out cookieValues))
                //{
                //    context.Request.Headers.Add("Cookie", new StringValues($"{DEFAULT_SESSION_COOKIE}={sessionToken.First()}"));
                //} else
                //{
                //    var existingCookies = cookieValues.ToList();
                //    existingCookies.Add($"{DEFAULT_SESSION_COOKIE}={sessionToken.First()}");
                //    context.Request.Headers.Remove("Cookie");
                //    context.Request.Headers.Add("Cookie",new StringValues(existingCookies.ToArray()));
                //}
            }
            await _next(context);
        }

        private static Task OnStartingCallback(object state)
        {
            var tuple = (Tuple<HttpContext, ApplicationSessionOptions>) state;
            var context = tuple.Item1;
            var options = tuple.Item2;
            var typedHeaders = context.Response.GetTypedHeaders();
            var sessionCookie = typedHeaders.SetCookie?.FirstOrDefault(c => c.Name == options.Cookie.Name);
            if (sessionCookie == null)
                return Task.FromResult(0);
            var sessionCookieValue = sessionCookie.Value;
            context.Response.Headers.Add(options.SessionTokenHeader, new StringValues(sessionCookie.Value.Value));
            context.Response.Headers.Remove("Set-Cookie");
            return Task.FromResult(0);
        }
    }
}