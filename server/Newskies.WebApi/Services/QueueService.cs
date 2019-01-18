using Microsoft.Extensions.Options;
using Newskies.QueueManager;
using Newskies.WebApi.Configuration;
using System;
using System.Threading.Tasks;

namespace Newskies.WebApi.Services
{
    public interface IQueueService
    {
        Task AddBookingToQueue(string queueCode, string note = "");
    }
    public class QueueService : ServiceBase, IQueueService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IQueueManager _client;

        public QueueService(ISessionBagService sessionBag, IQueueManager client, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddBookingToQueue(string queueCode, string note = "")
        {
            var booking = await _sessionBag.Booking();
            if (booking == null || string.IsNullOrEmpty(booking.RecordLocator))
                return;

            var signature = await _sessionBag.Signature();
            var queueItem = new BookingQueue
            {
                RecordLocator = booking.RecordLocator,
                QueueCode = queueCode,
                PriorityDate = DateTime.UtcNow,
                Notes = note
            };
            await _client.CommitBookingQueueAsync(new CommitBookingQueueRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                BookingQueue = queueItem
            });
        }
    }
}
