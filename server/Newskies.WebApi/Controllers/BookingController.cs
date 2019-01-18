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
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        // GET: api/Booking
        [HttpGet, RequireSessionBooking]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await _bookingService.GetSessionBooking());
        }

        // GET: api/Booking/{recordLocator}
        [HttpGet, Route("{recordLocator}")]
        public async Task<IActionResult> Get([FromRoute]RetrieveBookingRequest retrieveBookingRequest)
        {
            return new OkObjectResult(await _bookingService.RetrieveBooking(retrieveBookingRequest));
        }

        // POST: api/Booking
        [HttpPost, RequireSessionBooking, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]Contracts.CommitRequest request)
        {
            return new OkObjectResult(await _bookingService.CommitBooking());
        }

        // GET: api/Booking/Polling
        [HttpGet("[action]"), RequireSessionBooking, BookingStateSync]
        public async Task<IActionResult> Polling([FromQuery]bool resetCounter)
        {
            return new OkObjectResult(await _bookingService.GetPostCommitResults(resetCounter));
        }

        // GET: api/Booking/Itinerary
        [HttpGet("[action]"), RequireSessionBooking]
        public async Task<IActionResult> Itinerary()
        {
            return new OkObjectResult(await _bookingService.SendItinerary());
        }

        // POST: api/Booking/ClearState
        [HttpPost("[action]")]
        public async Task<IActionResult> ClearState()
        {
            await _bookingService.ClearStateBooking();
            return new OkResult();
        }

        [HttpPost("[action]"), RequireSessionBooking]
        public async Task<IActionResult> ApplyPromotion([FromBody]ApplyPromotionRequestData request)
        {
            return new OkObjectResult(await _bookingService.ApplyPromotion(request));
        }
    }
}
