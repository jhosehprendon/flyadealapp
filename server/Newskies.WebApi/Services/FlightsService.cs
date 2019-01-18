using System;
using System.Threading.Tasks;
using AutoMapper;
using Navitaire.WebServices.DataContracts.Booking;
using dto = Newskies.WebApi.Contracts;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.WebApi.Constants;
using System.Collections.Generic;
using Newskies.WebApi.Helpers;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using Microsoft.Extensions.Logging;
using Newskies.UtilitiesManager;
using System.Linq;
using System.Diagnostics;

namespace Newskies.WebApi.Services
{
    public interface IFlightsService
    {
        Task<dto.TripAvailabilityResponse> FindFlights(dto.TripAvailabilityRequest findFlightsRequest);
        Task<dto.LowFareTripAvailabilityResponse> FindLowFareFlights(dto.LowFareTripAvailabilityRequest request);
        Task<dto.PriceItinararyResponse> GetPriceItinerary(dto.SellJourneyByKeyRequestData sellJourneyByKeyRequestData);
        Task<dto.SellResponse> SellFlights(dto.SellJourneyByKeyRequestData sellJourneyByKeyRequestData);
        Task<dto.SellResponse> ChangeFlights(dto.ChangeFlightsRequest changeFlightsRequest);
    }

    public class FlightsService : ServiceBase, IFlightsService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IUserSessionService _userSessionService;
        private readonly IResourcesService _resourcesService;
        private readonly IBookingService _bookingService;
        private readonly IPassengersService _passengerService;
        private readonly ISsrsService _ssrsService;
        private readonly AvailabilitySettings _bookingSettings;
        private readonly PerformanceLoggingSettings _perfLogSettings;
        private readonly ILogger<FlightsService> _logger;
        private readonly IBookingManager _client;

        public FlightsService(ISessionBagService sessionBag, IUserSessionService userSessionService, IBookingManager client,
            IResourcesService resourcesService, IBookingService bookingService, ILogger<FlightsService> logger,
            IOptions<AppSettings> appSettings, IPassengersService passengerService, ISsrsService ssrsService) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _bookingSettings = appSettings.Value.AvailabilitySettings;
            _passengerService = passengerService ?? throw new ArgumentNullException(nameof(passengerService));
            _ssrsService = ssrsService ?? throw new ArgumentNullException(nameof(ssrsService));
            _perfLogSettings = appSettings.Value.PerformanceLoggingSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.TripAvailabilityResponse> FindFlights(dto.TripAvailabilityRequest request)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            var tripAvailabilityRequest = Mapper.Map<TripAvailabilityRequest>(request, m => m.AfterMap(SetFareTypes));
            var stopWatch = _perfLogSettings.EnableNavApiLogging ? Stopwatch.StartNew() : null;
            var response = await _client.GetAvailabilityVer2Async(new GetAvailabilityRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                TripAvailabilityRequest = tripAvailabilityRequest
            });
            //_navApiContractVer, false,
            //_navMsgContractVer, signature, tripAvailabilityRequest);
            if (_perfLogSettings.EnableNavApiLogging && stopWatch != null)
            {
                stopWatch.Stop();
                var msecs = stopWatch.ElapsedMilliseconds;
                _logger.WriteTimedLog(msecs, "BookingManager.GetAvailabilityVer2Async" + "|" + signature);
            }
            var avRequestWithInfant = Array.Find(request.AvailabilityRequests, p => Array.Find(
                p.PaxTypeCounts, q => q.PaxTypeCode == Global.INFANT_CODE && q.PaxCount > 0) != null);
            if (avRequestWithInfant != null)
            {
                var infantCount = Array.Find(avRequestWithInfant.PaxTypeCounts,
                    q => q.PaxTypeCode == Global.INFANT_CODE).PaxCount;
                foreach (var schedule in response.GetTripAvailabilityVer2Response.Schedules)
                    foreach (var market in schedule)
                        foreach (var journey in market.AvailableJourneys)
                        {
                            var clearFares = false;
                            foreach (var segment in journey.AvailableSegment)
                            {
                                foreach (var leg in segment.Legs)
                                {
                                    var legSSRInsufficient = Array.Find(leg.LegInfo.LegSSRs, p => p.SSRNestCode == Global.INFANT_CODE
                                        && p.SSRLid - p.SSRSold < infantCount);
                                    if (legSSRInsufficient != null)
                                    {
                                        clearFares = true;
                                        continue;
                                    }
                                }
                                if (clearFares)
                                    continue;
                            }
                            if (clearFares)
                                foreach (var segment in journey.AvailableSegment)
                                    segment.AvailableFares = new AvailableFare2[0];
                        }
            }
            var convertedResponse =
                Mapper.Map<dto.TripAvailabilityResponse>(response.GetTripAvailabilityVer2Response);
            return convertedResponse;

        }

        public async Task<dto.LowFareTripAvailabilityResponse> FindLowFareFlights(dto.LowFareTripAvailabilityRequest request)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            var lowFareTripAvailabilityRequest = Mapper.Map<LowFareTripAvailabilityRequest>(request, m => m.AfterMap(SetFareTypes));
            var stopWatch = _perfLogSettings.EnableNavApiLogging ? Stopwatch.StartNew() : null;
            var response = await _client.GetLowFareTripAvailabilityAsync(new GetLowFareTripAvailabilityRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                LowFareTripAvailabilityRequest = lowFareTripAvailabilityRequest
            });
            //_navApiContractVer, false,
            //_navMsgContractVer, signature, lowFareTripAvailabilityRequest);
            if (_perfLogSettings.EnableNavApiLogging && stopWatch != null)
            {
                stopWatch.Stop();
                var msecs = stopWatch.ElapsedMilliseconds;
                _logger.WriteTimedLog(msecs, "BookingManager.GetLowFareTripAvailabilityAsync" + "|" + signature);
            }
            var convertedResponse =
                Mapper.Map<dto.LowFareTripAvailabilityResponse>(response.LowFareTripAvailabilityResponse);
            return convertedResponse;
        }

        public async Task<dto.SellResponse> SellFlights(dto.SellJourneyByKeyRequestData sellJourneyByKeyRequestData)
        {
            if (string.IsNullOrEmpty(await _sessionBag.Signature()))
                await _userSessionService.AnonymousLogonUnique();
            var signature = await _sessionBag.Signature();
            if (await _sessionBag.Booking() != null)
            {
                // cancel previously selected flights (if any) for New Bookings only
                var booking = await _bookingService.GetSessionBooking();
                if (string.IsNullOrEmpty(booking.RecordLocator) && booking.Journeys != null && booking.Journeys.Length > 0 && booking.Passengers != null && booking.Passengers.Length > 0)
                {
                    // totally clear booking if pax type counts are different
                    if (!PaxCountsAreTheSame(booking, sellJourneyByKeyRequestData))
                    {
                        await _bookingService.ClearStateBooking();
                    }
                    // pax type counts are the same so just cancel the journeys
                    else
                    {
                        var cancelData = new CancelRequestData
                        {
                            CancelBy = CancelBy.Journey,
                            CancelJourney = new CancelJourney
                            {
                                CancelJourneyRequest = new CancelJourneyRequest
                                {
                                    Journeys = Mapper.Map<Journey[]>(booking.Journeys.ToArray())
                                }
                            }
                        };
                        try
                        {
                            await _client.CancelAsync(new CancelRequest
                            {
                                ContractVersion = _navApiContractVer,
                                MessageContractVersion = _navMsgContractVer,
                                Signature = signature,
                                EnableExceptionStackTrace = false,
                                CancelRequestData = cancelData
                            });
                        }
                        catch {
                            await _bookingService.ClearStateBooking();
                        }
                    }
                }
            }
            var flightsSellRequestData = new SellRequestData
            {
                SellBy = SellBy.JourneyBySellKey,
                SellJourneyByKeyRequest = new SellJourneyByKeyRequest
                {
                    SellJourneyByKeyRequestData = Mapper.Map<SellJourneyByKeyRequestData>(sellJourneyByKeyRequestData),
                }
            };
            var stopWatch = _perfLogSettings.EnableNavApiLogging ? Stopwatch.StartNew() : null;
            var sellFlightsResponse = await _client.SellAsync(new SellRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                SellRequestData = flightsSellRequestData
            });
            //_navApiContractVer, false, _navMsgContractVer,
            //signature, flightsSellRequestData);
            if (_perfLogSettings.EnableNavApiLogging && stopWatch != null)
            {
                stopWatch.Stop();
                var msecs = stopWatch.ElapsedMilliseconds;
                _logger.WriteTimedLog(msecs, "BookingManager.SellAsync" + "|" + signature);
            }
            var returnResponse = new dto.SellResponse();
            // Sell infant SSR
            var infantPaxCount = Array.Find(sellJourneyByKeyRequestData.PaxTypeCounts, p => p.PaxTypeCode == Global.INFANT_CODE);
            if (infantPaxCount != null && infantPaxCount.PaxCount > 0)
            {
                var infantSegmentSSRRequests = await CreateSegmentSSRRequests(sellJourneyByKeyRequestData);
                var infantSSRSellRequestData = new SellRequestData
                {
                    SellBy = SellBy.SSR,
                    SellSSR = new SellSSR
                    {
                        SSRRequest = new SSRRequest
                        {
                            CurrencyCode = sellJourneyByKeyRequestData.CurrencyCode,
                            SegmentSSRRequests = infantSegmentSSRRequests
                        }
                    }
                };
                var stopWatch2 = _perfLogSettings.EnableNavApiLogging ? Stopwatch.StartNew() : null;
                var sellSSRsResponse = await _client.SellAsync(new SellRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    SellRequestData = infantSSRSellRequestData
                });
                //_navApiContractVer, false, _navMsgContractVer,
                //signature, infantSSRSellRequestData);
                if (_perfLogSettings.EnableNavApiLogging && stopWatch != null)
                {
                    stopWatch2.Stop();
                    var msecs = stopWatch2.ElapsedMilliseconds;
                    _logger.WriteTimedLog(msecs, "BookingManager.SellAsync(INF)" + "|" + signature);
                }
                returnResponse = Mapper.Map<dto.SellResponse>(sellSSRsResponse);
            }
            else
            {
                returnResponse = Mapper.Map<dto.SellResponse>(sellFlightsResponse);
            }
            var stateBooking = await _bookingService.GetSessionBooking(true);
            await _sessionBag.SetBooking(stateBooking);
            if (string.IsNullOrEmpty(stateBooking.RecordLocator))
            {
                var paxesWithInfant = new List<dto.Passenger>();
                foreach (var pax in (await _sessionBag.Booking()).Passengers.ToList().FindAll(p => p.PassengerFees.ToList().Find(
                    f => f.FeeCode == Global.INFANT_CODE) != null))
                {
                    if (pax.Infant == null)
                    {
                        pax.Infant = new dto.PassengerInfant();
                        paxesWithInfant.Add(pax);
                    }
                }
                if (paxesWithInfant.Count() > 0)
                {
                    var updatePaxesWithInfantsResult = await _passengerService.UpdatePassengers(
                        new dto.UpdatePassengersRequestData { Passengers = paxesWithInfant.ToArray() });
                    if (updatePaxesWithInfantsResult.Error != null)
                    {
                        throw new Exception(updatePaxesWithInfantsResult.Error.ErrorText);
                    }
                    await _sessionBag.SetBooking(await _bookingService.GetSessionBooking(true));
                }
            }
            return returnResponse;
        }

        public async Task<dto.PriceItinararyResponse> GetPriceItinerary(dto.SellJourneyByKeyRequestData sellJourneyByKeyRequestData)
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                ? await _sessionBag.Signature()
                : await _userSessionService.GetAnonymousSharedSignature();
            CleanSellJourneyByKeyRequestData(sellJourneyByKeyRequestData);
            var infantSegmentSSRRequests = await CreateSegmentSSRRequests(sellJourneyByKeyRequestData);
            var priceItineraryRequest = new ItineraryPriceRequest
            {
                SSRRequest = new SSRRequest
                {
                    SegmentSSRRequests = infantSegmentSSRRequests,
                    CurrencyCode = sellJourneyByKeyRequestData.CurrencyCode
                },
                TypeOfSale = Mapper.Map<TypeOfSale>(sellJourneyByKeyRequestData.TypeOfSale),
                PriceItineraryBy = PriceItineraryBy.JourneyBySellKey,
                SellByKeyRequest = Mapper.Map<SellJourneyByKeyRequestData>(sellJourneyByKeyRequestData, m => m.AfterMap(SetFareTypes))
            };
            var stopWatch = _perfLogSettings.EnableNavApiLogging ? Stopwatch.StartNew() : null;
            var response = await _client.GetItineraryPriceAsync(new PriceItineraryRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = signature,
                EnableExceptionStackTrace = false,
                ItineraryPriceRequest = priceItineraryRequest
            });
            //_navApiContractVer, false,
            //_navMsgContractVer, signature, priceItineraryRequest);
            if (_perfLogSettings.EnableNavApiLogging && stopWatch != null)
            {
                stopWatch.Stop();
                var msecs = stopWatch.ElapsedMilliseconds;
                _logger.WriteTimedLog(msecs, "BookingManager.GetItineraryPriceAsync" + "|" + signature);
            }
            return Mapper.Map<dto.PriceItinararyResponse>(response.Booking);
        }

        public async Task<dto.SellResponse> ChangeFlights(dto.ChangeFlightsRequest changeFlightsRequest)
        {
            var signature = await _sessionBag.Signature();
            var booking = await _sessionBag.Booking();
            var sellFlightsResponse = new dto.SellResponse();
            for (var i = changeFlightsRequest.JourneySellKeys.Length - 1; i >= 0; i--)
            {
                if (changeFlightsRequest.JourneySellKeys[i] == null
                    || string.IsNullOrEmpty(changeFlightsRequest.JourneySellKeys[i].JourneySellKey)
                    || string.IsNullOrEmpty(changeFlightsRequest.JourneySellKeys[i].FareSellKey))
                {
                    continue;
                }
                var cancelResponse = await CancelFlight(i);
                if (cancelResponse.Error != null)
                {
                    //throw new dto.ResponseErrorException(
                    //    dto.Enumerations.ResponseErrorCode.ChangeFlightFailure, cancelResponse.Error.ErrorText);
                    return new dto.SellResponse { BookingUpdateResponseData = cancelResponse };
                }
            }
            for (var i = 0; i < changeFlightsRequest.JourneySellKeys.Length; i++)
            {
                if (changeFlightsRequest.JourneySellKeys[i] == null
                    || string.IsNullOrEmpty(changeFlightsRequest.JourneySellKeys[i].JourneySellKey)
                    || string.IsNullOrEmpty(changeFlightsRequest.JourneySellKeys[i].FareSellKey))
                {
                    continue;
                }
                var sellRequest = new dto.SellJourneyByKeyRequestData
                {
                    JourneySellKeys = new[] { changeFlightsRequest.JourneySellKeys[i] },
                    CurrencyCode = booking.CurrencyCode,
                    PaxTypeCounts = GetPaxTypeCounts(booking)
                };
                sellFlightsResponse = await SellFlights(sellRequest);
                if (sellFlightsResponse.BookingUpdateResponseData.Error != null)
                {
                    //throw new dto.ResponseErrorException(
                    //    dto.Enumerations.ResponseErrorCode.ChangeFlightFailure, sellFlightsResponse.BookingUpdateResponseData.Error.ErrorText);
                    return sellFlightsResponse;
                }
                var resellSSRsResponse = await _ssrsService.ResellSSRs(i, changeFlightsRequest.ResellSeatSSRs, changeFlightsRequest.WaiveSeatFee);
                if (resellSSRsResponse.Error != null)
                {
                    //throw new dto.ResponseErrorException(
                    //    dto.Enumerations.ResponseErrorCode.ChangeFlightFailure, resellSSRsResponse.Error.ErrorText);
                    return new dto.SellResponse { BookingUpdateResponseData = resellSSRsResponse };
                }
            }
            return sellFlightsResponse;
        }

        private async Task<SegmentSSRRequest[]> CreateSegmentSSRRequests(dto.SellJourneyByKeyRequestData sellJourneyByKeyRequestData)
        {
            var segmentSSRRequests = new List<SegmentSSRRequest>();
            var infantPaxCount = Array.Find(sellJourneyByKeyRequestData.PaxTypeCounts,
                    p => p.PaxTypeCode.Equals(Global.INFANT_CODE));
            if (infantPaxCount != null && infantPaxCount.PaxCount > 0)
            {
                var ssrs = await _resourcesService.GetSSRList(await _sessionBag.CultureCode());
                var infantSsr = Array.Find(ssrs.SSRList, p => p.SSRCode == Global.INFANT_CODE);
                var sellKeyList = Mapper.Map<SellKeyList[]>(sellJourneyByKeyRequestData.JourneySellKeys);
                var journeys = NewskiesHelper.GetJourneysFromSellKeys(sellKeyList).ToList();
                var paxNumbers = new List<int>();
                for (var i = 0; i < infantPaxCount.PaxCount; i++)
                    paxNumbers.Add(i);
                var journeySegmentLegIndexes = new List<Tuple<int, List<Tuple<int, int[]>>>>();
                for (var j = 0; j < journeys.Count(); j++)
                {
                    var segmentLegIndexes = new List<Tuple<int, int[]>>();
                    for (var s = 0; s < journeys[j].Segments.Length; s++)
                    {
                        segmentLegIndexes.Add(new Tuple<int, int[]>(s, null));
                    }
                    journeySegmentLegIndexes.Add(new Tuple<int, List<Tuple<int, int[]>>>(j, segmentLegIndexes));
                }
                segmentSSRRequests = NewskiesHelper.CreateSegmentSSRRequests(
                    journeys, Mapper.Map<SSR>(infantSsr), paxNumbers.ToArray(), 1, journeySegmentLegIndexes);
            }
            return segmentSSRRequests.ToArray();
        }

        private void CleanSellJourneyByKeyRequestData(dto.SellJourneyByKeyRequestData data)
        {
            var cleanedData = new List<dto.SellKeyList>();
            foreach (var sellKey in data.JourneySellKeys)
            {
                if (!string.IsNullOrEmpty(sellKey.JourneySellKey) && !string.IsNullOrEmpty(sellKey.FareSellKey))
                    cleanedData.Add(new dto.SellKeyList { JourneySellKey = sellKey.JourneySellKey, FareSellKey = sellKey.FareSellKey });
            }
            data.JourneySellKeys = cleanedData.ToArray();
        }

        private void SetFareTypes(object arg1, object arg2)
        {
            var type = arg2.GetType();
            if (type == typeof(TripAvailabilityRequest))
            {
                var tripAvailabilityRequest = (TripAvailabilityRequest)arg2;
                foreach (var availabilityRequest in tripAvailabilityRequest.AvailabilityRequests)
                    availabilityRequest.FareTypes = _bookingSettings.FareTypeCodes;
            }
            else if (type == typeof(LowFareTripAvailabilityRequest))
            {
                var lowFareTripAvailabilityRequest = (LowFareTripAvailabilityRequest)arg2;
                lowFareTripAvailabilityRequest.FareTypeList = _bookingSettings.FareTypeCodes;
            }
            else if (type == typeof(SellJourneyByKeyRequestData))
            {
                var sellJourneyByKeyRequestData = (SellJourneyByKeyRequestData)arg2;
                sellJourneyByKeyRequestData.TypeOfSale = new TypeOfSale
                {
                    FareTypes = _bookingSettings.FareTypeCodes
                };
            }
        }

        private async Task<dto.BookingUpdateResponseData> CancelFlight(int journeyIndex)
        {
            var booking = await _sessionBag.Booking();
            var flightsCancelRequestData = new CancelRequestData
            {
                CancelBy = CancelBy.Journey,
                CancelJourney = new CancelJourney
                {
                    CancelJourneyRequest = new CancelJourneyRequest
                    {
                        Journeys = new[] { Mapper.Map<Journey>(booking.Journeys[journeyIndex]) }
                    }
                }
            };
            var cancelResponse = await _client.CancelAsync(new CancelRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                CancelRequestData = flightsCancelRequestData
            });
            await _bookingService.GetSessionBooking(true);
            return Mapper.Map<dto.BookingUpdateResponseData>(cancelResponse.BookingUpdateResponseData);
        }

        private dto.PaxTypeCount[] GetPaxTypeCounts(dto.Booking booking)
        {
            var adtCount = booking.Passengers.ToList().FindAll(p => p.PassengerTypeInfo.PaxType == Global.ADULT_CODE).Count();
            var chdCount = booking.Passengers.ToList().FindAll(p => p.PassengerTypeInfo.PaxType == Global.CHILD_CODE).Count();
            var paxTypeCountsList = new List<dto.PaxTypeCount>();
            if (adtCount > 0) paxTypeCountsList.Add(new dto.PaxTypeCount { PaxTypeCode = Global.ADULT_CODE, PaxCount = (short)adtCount });
            if (chdCount > 0) paxTypeCountsList.Add(new dto.PaxTypeCount { PaxTypeCode = Global.CHILD_CODE, PaxCount = (short)chdCount });
            return paxTypeCountsList.ToArray();
        }

        private bool PaxCountsAreTheSame(dto.Booking booking, dto.SellJourneyByKeyRequestData sellJourneyByKeyRequestData)
        {
            var adtCount = sellJourneyByKeyRequestData.PaxTypeCounts.ToList().Find(p => p.PaxTypeCode == Global.ADULT_CODE)?.PaxCount;
            var chdCount = sellJourneyByKeyRequestData.PaxTypeCounts.ToList().Find(p => p.PaxTypeCode == Global.CHILD_CODE)?.PaxCount;
            var infCount = sellJourneyByKeyRequestData.PaxTypeCounts.ToList().Find(p => p.PaxTypeCode == Global.INFANT_CODE)?.PaxCount;
            var bookingAdtCount = booking.Passengers.ToList().FindAll(p => p.PassengerTypeInfo.PaxType == "ADT").Count();
            var bookingChdCount = booking.Passengers.ToList().FindAll(p => p.PassengerTypeInfo.PaxType == "CHD").Count();
            var bookingInfCount = booking.Passengers.ToList().FindAll(p => p.Infant != null).Count();
            return (adtCount.HasValue ? adtCount.Value : 0) == bookingAdtCount 
                && (chdCount.HasValue ? chdCount.Value : 0) == bookingChdCount
                && (infCount.HasValue ? infCount.Value : 0) == bookingInfCount;
        }
    }
}