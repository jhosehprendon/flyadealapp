using AutoMapper;
using Navitaire.WebServices.DataContracts.Booking;
using System;
using dto = Newskies.WebApi.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System.ServiceModel;

namespace Newskies.WebApi.Services
{
    public interface IContactsService
    {
        Task<dto.UpdateContactsResponse> UpdateContacts(dto.UpdateContactsRequestData request);
        Task<dto.GetBookingContactsResponse> GetContacts();
    }

    public class ContactsService : ServiceBase, IContactsService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IBookingService _bookingService;
        private readonly IBookingManager _client;

        public ContactsService(ISessionBagService sessionBag, IBookingService bookingService, IBookingManager client,
            IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.GetBookingContactsResponse> GetContacts()
        {
            var booking = await _bookingService.GetSessionBooking();
            return new dto.GetBookingContactsResponse { BookingContacts = booking.BookingContacts };
        }

        public async Task<dto.UpdateContactsResponse> UpdateContacts(dto.UpdateContactsRequestData request)
        {
            var booking = await _bookingService.GetSessionBooking();
            var sessionRoleCode = await _sessionBag.RoleCode();
            if (booking != null)
            {
                // Set customer number for booking contact for new member booking
                if (string.IsNullOrEmpty(booking.RecordLocator) && sessionRoleCode == _newskiesSettings.DefaultMemberRoleCode)
                {
                    request.BookingContactList[0].CustomerNumber = await _sessionBag.CustomerNumber();
                }
                // Set customer number for booking contact for existing booking
                else if (!string.IsNullOrEmpty(booking.RecordLocator))
                {
                    request.BookingContactList[0].CustomerNumber = booking.BookingContacts[0].CustomerNumber;
                }
            }

            var response = await _client.UpdateContactsAsync(new UpdateContactsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                updateContactsRequestData = Mapper.Map<UpdateContactsRequestData>(request)
            });
            //_navApiContractVer, false, _navMsgContractVer,
            //await _sessionBag.Signature(), Mapper.Map<UpdateContactsRequestData>(request));
            return Mapper.Map<dto.UpdateContactsResponse>(response);
        }
    }
}
