using Microsoft.AspNetCore.Mvc;
using dto = Newskies.WebApi.Contracts;
using Newskies.WebApi.Filters;
using Newskies.WebApi.Services;
using System;
using System.Threading.Tasks;

namespace Newskies.WebApi.Controllers
{
    [Authorization, RequireSessionBooking, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PaymentController : Controller
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        [HttpPost, BookingStateSync]
        public async Task<IActionResult> Post([FromBody]dto.AddPaymentToBookingRequestData request)
        {
            return new OkObjectResult(await _paymentsService.AddPaymentToBooking(request));
        }

        [HttpDelete, BookingStateSync]
        public async Task<IActionResult> Delete([FromQuery]dto.RemovePaymentFromBookingRequest removePaymentFromBookingRequest)
        {
            return new OkObjectResult(await _paymentsService.RemovePaymentFromBooking(removePaymentFromBookingRequest));
        }

        //[HttpGet("[action]"), BookingStateSync]
        //public async Task<IActionResult> Voucher([FromQuery]string voucherReference)
        //{
        //    return new OkObjectResult(await _paymentsService.GetVoucherInfo(voucherReference));
        //}
    }
}
