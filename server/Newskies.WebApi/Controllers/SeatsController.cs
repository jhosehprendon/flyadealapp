using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Filters;
using Newskies.WebApi.Services;
using Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Controllers
{
    [Authorization, RequireSessionBooking, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SeatsController : Controller
    {
        private readonly ISeatsService _seatsService;

        public SeatsController(ISeatsService seatsService)
        {
            _seatsService = seatsService ?? throw new ArgumentNullException(nameof(seatsService));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]SeatAvailabilityRequest request)
        {
            return new OkObjectResult(await _seatsService.GetSeatAvailability(request));
        }

        [HttpPost, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]AssignSeatRequest assignSeatRequest)
        {
            return new OkObjectResult(await _seatsService.AssignSeat(assignSeatRequest));
        }

        [HttpDelete, BookingStateSync]
        public async Task<IActionResult> Delete([FromQuery]AssignSeatRequest assignSeatRequest)
        {
            return new OkObjectResult(await _seatsService.UnAssignSeat(assignSeatRequest));
        }
    }
}
