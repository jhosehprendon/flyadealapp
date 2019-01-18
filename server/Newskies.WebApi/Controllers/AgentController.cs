using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;

namespace Newskies.WebApi.Controllers
{
    [Authorization, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IUserSessionService _userSessionService;
        private readonly IBookingService _bookingService;

        public AgentController(IAgentService agentService, IUserSessionService userSessionService, IBookingService bookingService)
        {
            _agentService = agentService ?? throw new ArgumentNullException(nameof(agentService));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CommitAgentRequestData commitAgentRequestData)
        {
            return new OkObjectResult(await _agentService.AddAgent(commitAgentRequestData));
        }

        [HttpPost, Route("{id:int}"), RequireSessionMasterAgent]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]Agent agent)
        {
            agent.AgentID = id;
            return new OkObjectResult(await _agentService.UpdateAgent(agent));
        }

        [HttpGet, RequireSessionAgent]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await _agentService.GetAgent());
        }

        [HttpGet, Route("{id:int}"), RequireSessionMasterAgent]
        public async Task<IActionResult> Get(int id)
        {
            return new OkObjectResult(await _agentService.GetAgent(id));
        }

        [HttpPost, Route("[action]"), RequireSessionAgent]
        public async Task<IActionResult> Person([FromBody]Person person)
        {
            return new OkObjectResult(await _agentService.UpdatePerson(person));
        }

        [HttpPost, Route("Person/{id:int}"), RequireSessionMasterAgent]
        public async Task<IActionResult> UpdatePerson([FromRoute]int id, [FromBody]Person person)
        {
            person.PersonID = id;
            return new OkObjectResult(await _agentService.UpdatePerson(person));
        }

        [HttpGet, Route("[action]"), RequireSessionAgent]
        public async Task<IActionResult> Bookings([FromQuery]FindBookingRequestData findBookingRequestData)
        {
            return new OkObjectResult(await _bookingService.FindBookings(findBookingRequestData));
        }

        [HttpPost, Route("[action]"), AgentRoleCheck]
        public async Task<IActionResult> Logon([FromBody]LogonRequestData logonRequestData)
        {
            return new OkObjectResult(await _userSessionService.Logon(logonRequestData));
        }

        [HttpPost, Route("[action]"), RequireSessionAgent]
        public async Task<IActionResult> Logoff()
        {
            return new OkObjectResult(await _userSessionService.Logout());
        }

        [HttpPost, Route("[action]"), RequireSessionAgent]
        public async Task<IActionResult> PasswordSet([FromBody]PasswordSetRequest passwordSetRequest)
        {
            await _agentService.PasswordSet(passwordSetRequest);
            return new OkResult();
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> PasswordSetAnonymously([FromBody]PasswordSetAnonymouslyRequest passwordSetAnonymouslyRequest)
        {
            await _userSessionService.SetPassword(passwordSetAnonymouslyRequest.LogonRequestData, passwordSetAnonymouslyRequest.PasswordSetRequest.NewPassword);
            return new OkResult();
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> PasswordReset([FromBody]PasswordResetRequest passwordResetRequest)
        {
            return new OkObjectResult(await _agentService.PasswordReset(passwordResetRequest));
        }

        [HttpGet, Route("[action]"), RequireSessionAgent]
        public async Task<IActionResult> Agents([FromQuery]FindAgentRequestData findAgentRequestData = null)
        {
            return new OkObjectResult(await _agentService.GetAgentList(findAgentRequestData));
        }
    }
}
