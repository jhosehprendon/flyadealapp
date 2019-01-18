using System;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Controllers
{
    [Authorization, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AgencyController : Controller
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IBookingService _bookingService;
        private readonly IAgencyService _agencyService;

        public AgencyController(IAgencyService agencyService, IUserSessionService userSessionService, IBookingService bookingService)
        {
            _agencyService = agencyService ?? throw new ArgumentNullException(nameof(agencyService));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        [HttpGet, RequireSessionAgent]
        public async Task<IActionResult> Get([FromQuery]GetOrganizationRequestData requestData)
        {
            return new OkObjectResult(await _agencyService.GetAgency(requestData));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CommitAgencyRequest commitAgencyRequest)
        {
            return new OkObjectResult(await _agencyService.AddAgency(commitAgencyRequest));
        }
    }
}
