using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;
using Newskies.WebApi.Services;

namespace Newskies.WebApi.Controllers
{
    [Authorization, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class FlightsController : Controller
    {
        private readonly IFlightsService _flightsService;

        public FlightsController(IFlightsService flightService)
        {
            _flightsService = flightService ?? throw new ArgumentNullException(nameof(flightService));
        }

        [HttpGet, FareTypes]
        public async Task<IActionResult> Get([FromQuery] TripAvailabilityRequest request)
        {
            return new OkObjectResult(await _flightsService.FindFlights(request));
        }

        [HttpPost]
        public async Task<IActionResult> Sell([FromBody] SellJourneyByKeyRequestData sellJourneyByKeyRequestData)
        {
            return new OkObjectResult(await _flightsService.SellFlights(sellJourneyByKeyRequestData));
        }

        [HttpGet("[action]"), FareTypes]
        public async Task<IActionResult> LowFares([FromQuery] LowFareTripAvailabilityRequest request)
        {
            return new OkObjectResult(await _flightsService.FindLowFareFlights(request));
        }

        [HttpGet("[action]"), FareTypes]
        public async Task<IActionResult> PriceItinerary([FromQuery] SellJourneyByKeyRequestData sellJourneyByKeyRequestData)
        {
            return new OkObjectResult(await _flightsService.GetPriceItinerary(sellJourneyByKeyRequestData));
        }

        [HttpPost("[action]"), RequireSessionBooking, BookingStateSync]
        public async Task<IActionResult> Change([FromBody] ChangeFlightsRequest changeFlightsRequest)
        {
            return new OkObjectResult(await _flightsService.ChangeFlights(changeFlightsRequest));
        }
    }
}