using System;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Filters;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Controllers
{
    [Authorization, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CheckinController : Controller
    {
        private readonly ICheckinService _checkinService;

        public CheckinController(ICheckinService checkinService)
        {
            _checkinService = checkinService ?? throw new ArgumentNullException(nameof(checkinService));
        }

        /*** Baggage Check-in not typically used for any of our WCI flows.
        [HttpPost("[action]"), RequireSessionBooking, BookingStateSync]
        public async Task<IActionResult> Baggage([FromBody]ProcessBaggageRequestData processBaggageRequestData)
        {
            return new OkObjectResult(await _checkinService.ProcessBaggage(processBaggageRequestData));
        }
        */

        [HttpPost, RequireSessionBooking, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]dto.CheckInPassengersRequestData checkInPassengersRequestData)
        {
            return new OkObjectResult(await _checkinService.Checkin(checkInPassengersRequestData));
        }

        [HttpPost("[action]"), RequireSessionBooking, BookingStateSync]
        public async Task<IActionResult> GetBarCodedBoardingPasses([FromBody]dto.GetBarCodedBoardingPassesRequest getBarCodedBoardingPassesRequest)
        {
            return new OkObjectResult(await _checkinService.GetBarCodedBoardingPasses(getBarCodedBoardingPassesRequest));
        }


        // TEST/INVESTIGATION PURPOSES
        //[HttpPost("[action]"), RequireSessionBooking, BookingStateSync]
        //public async Task<IActionResult> BoardPassenger(int paxIndex,int journeyIndex, int segmentIndex)
        //{
        //        return new OkObjectResult(await _checkinService.BoardPassenger(paxIndex, journeyIndex, segmentIndex));
        //}
        //[HttpPost("[action]"), RequireSessionBooking, BookingStateSync]
        //public async Task<IActionResult> Baggage(int paxIndex, int journeyIndex, int segmentIndex)
        //{
        //        return new OkObjectResult(await _checkinService.ProcessBaggage(paxIndex, journeyIndex, segmentIndex));
        //}
    }
}
