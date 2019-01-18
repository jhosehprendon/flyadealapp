using AutoMapper;
using Microsoft.Extensions.Options;
using Navitaire.WebServices.DataContracts.Booking;
using Newskies.VoucherManager;
using Newskies.WebApi.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Services
{
    public interface IPaymentsService
    {
        Task<dto.AddPaymentToBookingResponse> AddPaymentToBooking(dto.AddPaymentToBookingRequestData request);
        Task<dto.RemovePaymentFromBookingResponse> RemovePaymentFromBooking(dto.RemovePaymentFromBookingRequest removePaymentFromBookingRequest);
        Task<GetVoucherInfoResponse> GetVoucherInfo(string voucherReference);
    }

    public class PaymentsService : ServiceBase, IPaymentsService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IBookingService _bookingService;
        private readonly IBookingManager _bookingManagerClient;
        private readonly IVoucherManager _voucherManagerClient;

        public PaymentsService(ISessionBagService sessionBag, IUserSessionService userSessionService, IBookingManager bookingManagerClient,
            IVoucherManager voucherManagerClient, IBookingService bookingService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _bookingManagerClient = bookingManagerClient ?? throw new ArgumentNullException(nameof(bookingManagerClient));
            _voucherManagerClient = voucherManagerClient ?? throw new ArgumentNullException(nameof(voucherManagerClient));
        }

        public async Task<dto.AddPaymentToBookingResponse> AddPaymentToBooking(dto.AddPaymentToBookingRequestData request)
        {
            var addPaymentToBookingRequestData = Mapper.Map<AddPaymentToBookingRequestData>(request);
            var response = await _bookingManagerClient.AddPaymentToBookingAsync(new AddPaymentToBookingRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                addPaymentToBookingReqData = addPaymentToBookingRequestData

            });
            return Mapper.Map<dto.AddPaymentToBookingResponse>(response);
        }

        public async Task<dto.RemovePaymentFromBookingResponse> RemovePaymentFromBooking(dto.RemovePaymentFromBookingRequest removePaymentFromBookingRequest)
        {
            var booking = await _bookingService.GetSessionBooking();
            var payment = booking.Payments.ToList().Find(p => p.PaymentNumber == removePaymentFromBookingRequest.PaymentNumber);
            var response = await _bookingManagerClient.RemovePaymentFromBookingAsync(new RemovePaymentFromBookingRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                RemovePaymentFromBookingRequestData = new RemovePaymentFromBookingRequestData
                {
                    Payment = Mapper.Map<Payment>(payment)
                }
            });
            return Mapper.Map<dto.RemovePaymentFromBookingResponse>(response);
        }

        public async Task<GetVoucherInfoResponse> GetVoucherInfo(string voucherReference)
        {
            var getVoucherInfoResponse = await _voucherManagerClient.GetVoucherInfoAsync(new GetVoucherInfoRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                getVoucherInfoRequestData = new GetVoucherInfoRequestData
                {
                    VoucherReference = voucherReference
                }
            });
            return getVoucherInfoResponse;
        }
    }
}
