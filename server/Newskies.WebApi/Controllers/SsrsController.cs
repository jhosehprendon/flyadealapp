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
    public class SsrsController : Controller
    {
        private readonly ISsrsService _ssrsService;

        public SsrsController(ISsrsService ssrsService)
        {
            _ssrsService = ssrsService ?? throw new ArgumentNullException(nameof(ssrsService));
        }

        // GET: api/Ssrs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await _ssrsService.GetSSRAvailabilityForBooking());
        }

        // POST: api/Ssrs
        [HttpPost, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]SellSSRRequest sellSSRRequest)
        {
            return new OkObjectResult(await _ssrsService.SellSSR(sellSSRRequest));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete, BookingStateSync]
        public async Task<IActionResult> Delete([FromQuery]CancelSSRRequest cancelRequest)
        {
            return new OkObjectResult(await _ssrsService.CancelSSR(cancelRequest));
        }
    }
}
