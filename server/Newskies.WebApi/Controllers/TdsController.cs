using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Newskies.WebApi.Controllers
{
    [ApiExceptionFilter]
    [Route("[controller]")]
    public class TdsController : Controller
    {
        private IOptions<AppSettings> _options;

        public TdsController(IOptions<AppSettings> options)
        {
            _options = options;
        }

        [HttpPost("[action]")]
        public IActionResult PayFort([FromForm]PayFortTdsCallbackPayload payload)
        {
            StringValues tokenValues;
            if (payload == null || string.IsNullOrEmpty(payload.response_message) || !HttpContext.Request.Query.TryGetValue("t", out tokenValues))
            {
                return new NotFoundObjectResult("Not Found");
            }
            var sessionToken = Encoding.UTF8.GetString(Convert.FromBase64String(tokenValues.First()));
            var redirectUrlTemplate = _options.Value.PaymentSettings.Client3DSRedirectUrlTemplate ?? "";
            var appPath = HttpContext.Request.PathBase.HasValue ? HttpContext.Request.PathBase.Value : string.Empty;
            var redirectUrl = string.Format(redirectUrlTemplate, HttpContext.Request.Scheme, HttpContext.Request.Host, appPath, WebUtility.UrlEncode(sessionToken), WebUtility.UrlEncode(payload.response_message.ToLowerInvariant()));
            return new RedirectResult(redirectUrl);
        }
    }
}
