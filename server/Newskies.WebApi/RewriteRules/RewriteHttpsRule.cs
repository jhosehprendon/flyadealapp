using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Net;

namespace Newskies.WebApi.RewriteRules
{
    public class RewriteHttpsRule : IRule
    {
        private readonly int _statusCode;

        public RewriteHttpsRule()
        {
            _statusCode = (int)HttpStatusCode.MovedPermanently;
        }

        public void ApplyRule(RewriteContext context)
        {
            HttpRequest req = context.HttpContext.Request;
            var header = req.Headers["X-Forwarded-Proto"];
            var requestProxiedByAmazonELB = header.Count() > 0 && !string.IsNullOrEmpty(header[0]);
            var clientRequestedHttps = header.Count() > 0 && header[0].ToLower() == "https";
            if (requestProxiedByAmazonELB && !clientRequestedHttps &&
                !req.Path.Value.Contains("/api/") && (req.Path.Value == "" || req.Path.Value == "/"))
            {
                string url = "https://" + req.Host + req.Path;
                var response = context.HttpContext.Response;
                response.StatusCode = _statusCode;
                response.Headers[HeaderNames.Location] = url;
                context.Result = RuleResult.EndResponse; // Do not continue processing the request  
            }
        }
    }
}
