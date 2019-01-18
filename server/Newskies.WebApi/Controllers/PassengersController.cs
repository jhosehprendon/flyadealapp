using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;

namespace Newskies.WebApi.Controllers
{
    [Authorization, RequireSessionBooking, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PassengersController : Controller
    {
        private readonly IPassengersService _passengersService;

        public PassengersController(IPassengersService passengersService)
        {
            _passengersService = passengersService ?? throw new ArgumentNullException(nameof(passengersService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await _passengersService.GetPassengers());
        }

        [HttpPost, PaxDetailsUpdateCheckFlightFlown, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]UpdatePassengersRequestData requestData)
        {
            return new OkObjectResult(await _passengersService.UpdatePassengers(requestData));
        }
    }
}