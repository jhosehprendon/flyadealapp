using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System;
using System.Threading.Tasks;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using Navitaire.WebServices.DataContracts.Booking;
using System.Collections.Generic;

namespace Newskies.WebApi.Services
{
    public interface IBookingCommentsService
    {
        Task AddBookingComments(List<string> comments);
    }

    public class BookingCommentsService : ServiceBase, IBookingCommentsService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IBookingService _bookingService;
        private readonly IBookingManager _client;

        public BookingCommentsService(ISessionBagService sessionBag, IBookingService bookingService, IBookingManager client,
            IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddBookingComments(List<string> comments)
        {
            var booking = await _bookingService.GetSessionBooking(true);
            if (string.IsNullOrEmpty(booking.RecordLocator) || comments == null || comments.Count == 0)
            {
                return;
            }
            var bookingComments = new List<BookingComment>();
            foreach (var comment in comments)
            {
                bookingComments.Add(new BookingComment
                {
                    CreatedDate = DateTime.UtcNow,
                    CommentType = CommentType.Default,
                    CommentText = comment
                });
            }
            await _client.AddBookingCommentsAsync(new AddBookingCommentsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                AddBookingCommentsReqData = new AddBookingCommentsRequestData
                {
                    RecordLocator = booking.RecordLocator,
                    BookingComments = bookingComments.ToArray()
                }

            });
        }
    }
}
