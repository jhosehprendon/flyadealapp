using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;
using Newskies.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Controllers
{
    [Authorization, RequireSessionBooking, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class FeeController : Controller
    {
        private readonly IFeeService _feeService;

        public FeeController(IFeeService feeService)
        {
            _feeService = feeService as IFeeService ?? throw new ArgumentNullException(nameof(feeService));
        }

        [HttpPost, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]SellFeeRequestData sellFeeRequestData)
        {
            return new OkObjectResult(await _feeService.SellFee(sellFeeRequestData));
        }

        [HttpDelete, BookingStateSync]
        public async Task<IActionResult> Delete([FromQuery]CancelFeeRequestData cancelFeeRequestData)
        {
            return new OkObjectResult(await _feeService.CancelFee(cancelFeeRequestData));
        }
    }
}
