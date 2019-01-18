using AutoMapper;
using Microsoft.Extensions.Options;
using Navitaire.WebServices.DataContracts.Booking;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.WebApi.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.UtilitiesManager;

namespace Newskies.WebApi.Services
{
    public interface ISsrsService
    {
        Task<dto.GetSSRAvailabilityForBookingResponse> GetSSRAvailabilityForBooking();
        Task<dto.SellResponse> SellSSR(dto.SellSSRRequest sellSSRRequest);
        Task<dto.CancelResponse> CancelSSR(dto.CancelSSRRequest cancelSSRRequest);
        Task<dto.BookingUpdateResponseData> ResellSSRs(int journeyNumber, bool resellSeatSSRs = true, bool waiveSeatFee = true);
    }

    public class SsrsService : ServiceBase, ISsrsService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IResourcesService _resourcesService;
        private readonly IBookingManager _client;

        public SsrsService(ISessionBagService sessionBag, IResourcesService resourcesService, IBookingManager client,
            IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.GetSSRAvailabilityForBookingResponse> GetSSRAvailabilityForBooking()
        {
            var legKeys = new List<LegKey>();
            foreach (var journey in (await _sessionBag.Booking()).Journeys)
                foreach (var segment in journey.Segments)
                    foreach (var leg in segment.Legs)
                        legKeys.Add(new LegKey
                        {
                            ArrivalStation = segment.ArrivalStation,
                            DepartureStation = segment.DepartureStation,
                            CarrierCode = segment.FlightDesignator.CarrierCode.Trim(),
                            DepartureDate = segment.STD,
                            FlightNumber = segment.FlightDesignator.FlightNumber.Trim(),
                            OpSuffix = segment.FlightDesignator.OpSuffix.Trim()
                        });
            var ssrAvailabilityForBookingRequest = new SSRAvailabilityForBookingRequest
            {
                CurrencyCode = (await _sessionBag.Booking()).CurrencyCode,
                SSRAvailabilityMode = SSRAvailabilityMode.NonBundledSSRs,
                InventoryControlled = true,
                NonInventoryControlled = true,
                NonSeatDependent = true,
                SeatDependent = true,
                PassengerNumberList = (await _sessionBag.Booking()).Passengers.Select(p => p.PassengerNumber).ToArray(),
                SegmentKeyList = legKeys.ToArray()
            };
            var response = await _client.GetSSRAvailabilityForBookingAsync(new GetSSRAvailabilityForBookingRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                SSRAvailabilityForBookingRequest = ssrAvailabilityForBookingRequest
            });
            //_navApiContractVer, false, _navMsgContractVer,
            //await _sessionBag.Signature(), ssrAvailabilityForBookingRequest);
            return Mapper.Map<dto.GetSSRAvailabilityForBookingResponse>(response);
        }

        public async Task<dto.SellResponse> SellSSR(dto.SellSSRRequest sellSSRRequest)
        {
            var data = sellSSRRequest.SSRRequestData;
            var journeySegmentLegIndexes = new List<Tuple<int, List<Tuple<int, int[]>>>> {
                new Tuple<int, List<Tuple<int, int[]>>>(data.JourneyIndex, new List<Tuple<int, int[]>> {
                    new Tuple<int, int[]>(data.SegmentIndex, data.LegIndex.HasValue ? new[] { data.LegIndex.Value } : null)})};
            var signature = await _sessionBag.Signature();
            var ssrs = await _resourcesService.GetSSRList(await _sessionBag.CultureCode());
            var ssr = ssrs.SSRList.FirstOrDefault(p => p.SSRCode == data.SSRCode);//Array.Find(ssrs.SSRList, p => p.SSRCode == data.SSRCode);
            var navJourneys = Mapper.Map<Journey[]>((await _sessionBag.Booking()).Journeys);
            var segmentSSRRequests = NewskiesHelper.CreateSegmentSSRRequests(navJourneys.ToList(), Mapper.Map<SSR>(ssr),
                new[] { data.PaxNumber }, data.SSRCount, journeySegmentLegIndexes, note: data.Note);
            var ssrSellRequestData = new SellRequestData
            {
                SellBy = SellBy.SSR,
                SellSSR = new SellSSR
                {
                    SSRRequest = new SSRRequest
                    {
                        CurrencyCode = (await _sessionBag.Booking()).CurrencyCode,
                        SegmentSSRRequests = segmentSSRRequests.ToArray()
                    }
                }
            };
            var sellSSRsResponse = await _client.SellAsync(new SellRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                SellRequestData = ssrSellRequestData
            });
            //_navApiContractVer, false, _navMsgContractVer, signature, ssrSellRequestData);
            return Mapper.Map<dto.SellResponse>(sellSSRsResponse);
        }

        public async Task<dto.CancelResponse> CancelSSR(dto.CancelSSRRequest cancelSSRRequest)
        {
            var signature = await _sessionBag.Signature();
            var data = cancelSSRRequest.SSRRequestData;
            var journeySegmentLegIndexes = new List<Tuple<int, List<Tuple<int, int[]>>>> {
                new Tuple<int, List<Tuple<int, int[]>>>(data.JourneyIndex, new List<Tuple<int, int[]>> {
                    new Tuple<int, int[]>(data.SegmentIndex, data.LegIndex.HasValue ? new[] { data.LegIndex.Value } : null)})};
            var ssrs = await _resourcesService.GetSSRList(await _sessionBag.CultureCode());
            var ssr = Array.Find(ssrs.SSRList, p => p.SSRCode == data.SSRCode);
            var existingPaxSsrs = Array.FindAll((await _sessionBag.Booking()).Journeys[data.JourneyIndex].Segments[data.SegmentIndex].PaxSSRs,
                p => p.SSRCode == data.SSRCode && p.PassengerNumber == data.PaxNumber && (data.Note != null ? data.Note == p.Note : true));
            var ssrNumbersAll = existingPaxSsrs.Select(p => p.SSRNumber).ToList();
            var ssrNumbersToCancel = ssrNumbersAll.GetRange(ssrNumbersAll.Count() - data.SSRCount, data.SSRCount);
            var ssrNumbersToCancelDict = new Dictionary<int, List<short>>();
            ssrNumbersToCancelDict.Add(data.PaxNumber, ssrNumbersToCancel.ToList());
            var navJourneys = Mapper.Map<Journey[]>((await _sessionBag.Booking()).Journeys);
            var segmentSSRRequests = NewskiesHelper.CreateSegmentSSRRequests(navJourneys.ToList(), Mapper.Map<SSR>(ssr),
                new[] { data.PaxNumber }, data.SSRCount, journeySegmentLegIndexes, ssrNumbersToCancelDict, data.Note);
            var cancelRequestData = new CancelRequestData
            {
                CancelBy = CancelBy.SSR,
                CancelSSR = new CancelSSR
                {
                    SSRRequest = new SSRRequest
                    {
                        CurrencyCode = (await _sessionBag.Booking()).CurrencyCode,
                        SegmentSSRRequests = segmentSSRRequests.ToArray()
                    }
                }
            };
            var cancelSSRsResponse = await _client.CancelAsync(new CancelRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                CancelRequestData = cancelRequestData
            });
            // _navApiContractVer, false, _navMsgContractVer, signature, cancelRequestData);
            return Mapper.Map<dto.CancelResponse>(cancelSSRsResponse);
        }

        public async Task<dto.BookingUpdateResponseData> ResellSSRs(int journeyNumber, bool resellSeatSSRs = false, bool waiveSeatFee = false)
        {
            var signature = await _sessionBag.Signature();
            var response = await _client.ResellSSRAsync(new ResellSSRRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                ResellSSR = new ResellSSR
                {
                    JourneyNumber = journeyNumber,
                    ResellSeatSSRs = resellSeatSSRs,
                    ResellSSRs = true,
                    WaiveSeatFee = waiveSeatFee
                }
            });
            return Mapper.Map<dto.BookingUpdateResponseData>(response.BookingUpdateResponseData);
        }
    }
}
