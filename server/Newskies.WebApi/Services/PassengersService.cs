using AutoMapper;
using Microsoft.Extensions.Options;
using Navitaire.WebServices.DataContracts.Booking;
using Newskies.WebApi.Configuration;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Services
{
    public interface IPassengersService
    {
        Task<dto.BookingUpdateResponseData> UpdatePassengers(dto.UpdatePassengersRequestData requestData);
        Task<dto.GetPassengersResponse> GetPassengers();
    }

    public class PassengersService : ServiceBase, IPassengersService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IBookingService _bookingService;
        private readonly IBookingManager _client;

        public PassengersService(ISessionBagService sessionBag, IUserSessionService userSessionService, IBookingManager client,
            IBookingService bookingService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.GetPassengersResponse> GetPassengers()
        {
            var booking = await _bookingService.GetSessionBooking();
            return new dto.GetPassengersResponse { Passengers = booking.Passengers };
        }

        public async Task<dto.BookingUpdateResponseData> UpdatePassengers(dto.UpdatePassengersRequestData requestData)
        {
            var mappedRequest = Mapper.Map<UpdatePassengersRequestData>(requestData);
            var response = await _client.UpdatePassengersAsync(new UpdatePassengersRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                updatePassengersRequestData = mappedRequest
            });
            //_navApiContractVer, false, _navMsgContractVer,
            //await _sessionBag.Signature(), mappedRequest);
            var mappedResponse = Mapper.Map<dto.BookingUpdateResponseData>(response.BookingUpdateResponseData);
            return mappedResponse;
        }
    }
}
