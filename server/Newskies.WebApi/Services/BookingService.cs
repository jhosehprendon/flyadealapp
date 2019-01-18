using AutoMapper;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System;
using System.Threading.Tasks;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using Navitaire.WebServices.DataContracts.Booking;
using dto = Newskies.WebApi.Contracts;
using System.Linq;
using System.Text;

namespace Newskies.WebApi.Services
{
    public interface IBookingService
    {
        /// <summary>
        /// Return the Booking stored in local session. If ISessionBagService.BookingStateInSync is false, a
        /// call is made first to synchronise the Booking in local session with the Booking State at Navitaire's 
        /// side before returning the Booking.
        /// </summary>
        /// <param name="overrideLocalSyncCheck">Set to true to ignore checking ISessionBag.BookingStateInSync</param>
        /// <returns></returns>
        Task<dto.Booking> GetSessionBooking(bool overrideLocalSyncCheck = false);
        Task<dto.RetrieveBookingResponse> RetrieveBooking(dto.RetrieveBookingRequest retrieveBookingRequest);
        Task ClearStateBooking();
        Task<dto.CommitResponse> CommitBooking();
        Task<AddInProcessPaymentToBookingResponse> AddInProcessPaymentToBooking();
        Task<dto.GetPostCommitResultsResponse> GetPostCommitResults(bool resetCounter = false);
        Task<bool> SendItinerary();
        Task<dto.FindBookingResponseData> FindBookings(dto.FindBookingRequestData findBookingRequestData);
        Task<dto.ApplyPromotionResponse> ApplyPromotion(dto.ApplyPromotionRequestData request);
    }

    public class BookingService : ServiceBase, IBookingService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IUserSessionService _userSessionService;
        private readonly CommitBookingSettings _commitBookingSettings;
        private readonly IBookingManager _client;

        public BookingService(ISessionBagService sessionBag, IUserSessionService userSessionService, IBookingManager client,
            IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _commitBookingSettings = appSettings.Value.CommitBookingSettings;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.Booking> GetSessionBooking(bool overrideLocalSyncCheck = false)
        {
            if (await _sessionBag.BookingStateInSync() && !overrideLocalSyncCheck)
                return await _sessionBag.Booking();
            var getBookingFromStateResponse = await _client.GetBookingFromStateAsync(new GetBookingFromStateRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false
            });
            //_navApiContractVer, false, _navMsgContractVer, await _sessionBag.Signature());
            var mappedBooking = Mapper.Map<dto.Booking>(getBookingFromStateResponse.BookingData);
            await _sessionBag.SetBooking(mappedBooking);
            return mappedBooking;
        }

        public async Task<dto.RetrieveBookingResponse> RetrieveBooking(dto.RetrieveBookingRequest retrieveBookingRequest)
        {
            if (string.IsNullOrEmpty(await _sessionBag.Signature()))
                await _userSessionService.AnonymousLogonUnique();
            var getBookingRequestData = new GetBookingRequestData
            {
                GetBookingBy = GetBookingBy.RecordLocator,
                GetByRecordLocator = Mapper.Map<GetByRecordLocator>(retrieveBookingRequest)
            };
            var getBookingResponse = await _client.GetBookingAsync(new GetBookingRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                GetBookingReqData = getBookingRequestData
            });
            if (getBookingResponse.Booking == null)
            {
                await _sessionBag.SetBooking(null);
                throw new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.BookingNotFound, new[] { "Booking not found. " });
            }
            var booking = Mapper.Map<dto.Booking>(getBookingResponse.Booking);
            await _sessionBag.SetBooking(booking);
            return new dto.RetrieveBookingResponse { Booking = booking };
        }

        /// <summary>
        /// Clears the Booking in the state if the one was created.
        /// </summary>
        /// <returns></returns>
        public async Task ClearStateBooking()
        {
            if (!string.IsNullOrEmpty(await _sessionBag.Signature()) && await _sessionBag.Booking() != null)
            {
                await _client.ClearAsync(new ClearRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = await _sessionBag.Signature(),
                    EnableExceptionStackTrace = false
                });
                //_navApiContractVer, false, _navMsgContractVer, await _sessionBag.Signature());
                await GetSessionBooking(true);
            }
        }

        public async Task<dto.CommitResponse> CommitBooking()
        {
            //var getBookingResp = await _client.GetBookingFromStateAsync(new GetBookingFromStateRequest
            //{
            //    ContractVersion = _navApiContractVer,
            //    MessageContractVersion = _navMsgContractVer,
            //    Signature = await _sessionBag.Signature(),
            //    EnableExceptionStackTrace = false
            //});
            var commitRequest = new CommitRequestData { DistributeToContacts = true }; //, Booking = getBookingResp != null && getBookingResp.BookingData != null ? getBookingResp.BookingData : null };
            var commitResult = await _client.CommitAsync(new CommitRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                BookingRequest = commitRequest
            });
            //_navApiContractVer,
            //false, _navMsgContractVer, await _sessionBag.Signature(), new CommitRequestData());
            return Mapper.Map<dto.CommitResponse>(commitResult);
        }

        public async Task<dto.GetPostCommitResultsResponse> GetPostCommitResults(bool resetCounter = false)
        {
            // if call doesn't qualify for another nav call, then just return result of last call
            if ((await _sessionBag.PostCommitResultsResponse()) != null &&
                    (((await _sessionBag.PostCommitResultsResponse()).SessionPaymentQueryCount >= _commitBookingSettings.MaxQueryCount && !resetCounter) ||
                    (DateTime.UtcNow - (await _sessionBag.PostCommitResultsResponse()).PaymentQueryLastCall).TotalSeconds < _commitBookingSettings.RepeatQueryIntervalSecs))
                return await _sessionBag.PostCommitResultsResponse();
            if (resetCounter)
            {
                await _sessionBag.SetPostCommitResultsResponse(null);
            }
            var response = await _client.GetPostCommitResultsAsync(new GetPostCommitResultsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                IncludeBookingDelta = true
            });
            //_navApiContractVer, false,
            //_navMsgContractVer, await _sessionBag.Signature(), true);
            await _sessionBag.SetPostCommitResultsResponse(Mapper.Map<dto.GetPostCommitResultsResponse>(response, m => m.AfterMap(SetPollingValues)));
            return await _sessionBag.PostCommitResultsResponse();
        }

        public async Task<bool> SendItinerary()
        {
            var booking = await _sessionBag.Booking();
            if (booking != null && !string.IsNullOrEmpty(booking.RecordLocator))
            {
                await _client.SendItineraryAsync(new SendItineraryRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = await _sessionBag.Signature(),
                    EnableExceptionStackTrace = false,
                    RecordLocatorReqData = booking.RecordLocator
                });
                return true;
            }
            return false;
        }

        public async Task<AddInProcessPaymentToBookingResponse> AddInProcessPaymentToBooking()
        {
            var booking = await GetSessionBooking(true);
            var payment = booking.Payments.LastOrDefault();
            if (payment == null)
                return null;
            var response = await _client.AddInProcessPaymentToBookingAsync(new AddInProcessPaymentToBookingRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                Payment = Mapper.Map<Payment>(payment)
            });
            //_navApiContractVer, false,
            //_navMsgContractVer, await _sessionBag.Signature(), Mapper.Map<Payment>(payment));
            return response;
        }

        public async Task<dto.FindBookingResponseData> FindBookings(dto.FindBookingRequestData findBookingRequestData)
        {
            var pageSize = findBookingRequestData != null ? findBookingRequestData.PageSize : 10;
            var requestData = Mapper.Map<FindBookingRequestData>(findBookingRequestData);
            requestData.PageSize = (short)pageSize;
            var response = await _client.FindBookingAsync(new FindBookingRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                FindBookingRequestData = requestData
            });
            return Mapper.Map<dto.FindBookingResponseData>(response.FindBookingRespData);
        }

        public async Task<dto.ApplyPromotionResponse> ApplyPromotion(dto.ApplyPromotionRequestData request)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            var reqData = Mapper.Map<ApplyPromotionRequestData>(request);
            var response = await _client.ApplyPromotionAsync(new ApplyPromotionRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                Signature = signature,
                ApplyPromotionReqData = reqData
            });
            await GetSessionBooking(true);
            var convertedResponse =
                Mapper.Map<dto.ApplyPromotionResponse>(response);
            return convertedResponse;
        }

        private void SetPollingValues(object arg1, object arg2)
        {
            var getPostCommitResultsResponse = (dto.GetPostCommitResultsResponse)arg2;
            getPostCommitResultsResponse.MaxQueryCount = _commitBookingSettings.MaxQueryCount;
            getPostCommitResultsResponse.RepeatQueryIntervalSecs = _commitBookingSettings.RepeatQueryIntervalSecs;
            var posCommitResultsResponse = Task.Run(async () => await _sessionBag.PostCommitResultsResponse()).Result;
            getPostCommitResultsResponse.SessionPaymentQueryCount = posCommitResultsResponse != null ?
                posCommitResultsResponse.SessionPaymentQueryCount + 1 : 1;
            getPostCommitResultsResponse.PaymentQueryLastCall = DateTime.UtcNow;
        }
    }
}
