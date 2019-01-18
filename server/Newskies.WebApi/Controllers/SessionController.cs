using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Filters;

namespace Newskies.WebApi.Controllers
{
    [ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SessionController : Controller
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IUserSessionService _userSessionService;

        public SessionController(ISessionBagService sessionBag, IUserSessionService userSessionService)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
        }

        // POST api/session
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _sessionBag.Clear();
            await _sessionBag.Initialise();
            return await Task.FromResult<IActionResult>(new NoContentResult());
        }

        /// <summary>
        /// Keeps session alive.
        /// </summary>
        /// <returns></returns>
        [Authorization, HttpGet]
        public async Task<IActionResult> Get()
        {
            await _userSessionService.KeepAlive();
            return new OkObjectResult(await _userSessionService.GetSessionInfo());
        }

        // DELETE api/session
        [Authorization, HttpDelete]
        public async Task<IActionResult> HttpDelete()
        {
            var isAgentSession = !await _userSessionService.IsAnonymousSession();
            if (isAgentSession)
            {
                await _userSessionService.Logout();
            }
            await _sessionBag.Clear();
            return new OkResult();
        }
    }
}