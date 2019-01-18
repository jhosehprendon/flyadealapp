using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Newskies.WebApi.Filters;
using System.IO;

namespace Newskies.WebApi.Controllers
{
    public class HomeController
    {
        private static readonly byte[] _indexFileBytes;

        static HomeController()
        {
            _indexFileBytes = GetIndexPageBytes();
        }

        internal static byte[] GetIndexPageBytes()
        {
            var path = Path.Combine(ApplicationEnvironment.ApplicationBasePath, "static", "index.html");
            return File.Exists(path) ? File.ReadAllBytes(path) : new byte[0];
        }

        /// <summary>
        /// Action to load index.html of the client application. 
        /// It's assumed the client app is located in "static" folder
        /// </summary>
        /// <returns></returns>
        [HttpGet, RequireTrailingSlash, ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public IActionResult Spa()
        {
            return new FileContentResult(_indexFileBytes, "text/html");
        }
    }
}
