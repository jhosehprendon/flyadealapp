using AutoMapper;
using Microsoft.Extensions.Options;
using nskop = Navitaire.WebServices.DataContracts.Operation;
using Newskies.WebApi.Configuration;
using System;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;
using System.Collections.Generic;
using System.Linq;
using Newskies.WebApi.Contracts;
using Navitaire.WebServices.DataContracts.Common.Enumerations;

namespace Newskies.WebApi.Services
{
    public interface ICheckinService
    {
        Task<dto.CheckInPassengersResponse> Checkin(CheckInPassengersRequestData requestData);
        Task<dto.GetBarCodedBoardingPassesResponse> GetBarCodedBoardingPasses(dto.GetBarCodedBoardingPassesRequest getBarCodedBoardingPassesRequest);

        //Task<object> ProcessBaggage(int paxIndex, int journeyIndex, int segmentIndex);
        //Task<object> BoardPassenger(int paxIndex, int journeyIndex, int segmentIndex);
    }

    public class CheckinService : ServiceBase, ICheckinService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IOperationManager _client;

        public CheckinService(ISessionBagService sessionBag, IOperationManager client, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.CheckInPassengersResponse> Checkin(dto.CheckInPassengersRequestData requestData)
        {
            var booking = await _sessionBag.Booking();
            foreach (var req in requestData.CheckInMultiplePassengerRequestList)
            {
                var segment = booking.Journeys[req.JourneyIndex].Segments[req.SegmentIndex];
                req.RecordLocator = booking.RecordLocator;
                req.InventoryLegKey = new dto.InventoryLegKey
                {
                    ArrivalStation = segment.ArrivalStation,
                    DepartureStation = segment.DepartureStation,
                    CarrierCode = segment.FlightDesignator.CarrierCode,
                    FlightNumber = segment.FlightDesignator.FlightNumber,
                    OpSuffix = segment.FlightDesignator.OpSuffix,
                    DepartureDate = segment.STD
                };
                req.InventoryLegKeyDepartureDateTime = segment.STD;
                var list = new List<dto.CheckInPaxRequest>();
                foreach (var paxNum in req.PassengerNumbers)
                {
                    var bookingPax = Array.Find(booking.Passengers, p => p.PassengerNumber == paxNum);
                    var paxName = bookingPax.Names[0];
                    list.Add(new dto.CheckInPaxRequest
                    {
                        PassengerNumber = paxNum,
                        Name = new dto.Name
                        {
                            Title = paxName.Title,
                            FirstName = paxName.FirstName,
                            LastName = paxName.LastName,
                            MiddleName = null
                        }
                    });
                }
                req.CheckInPaxRequestList = list.ToArray();
            }
            var mappedRequest = Mapper.Map<nskop.CheckInPassengersRequestData>(requestData);
            var response = await _client.CheckInPassengersAsync(new CheckInPassengersRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                CheckInMultiplePassengersRequest = mappedRequest
            });

            var errorList = from checkInMultiplePassengersResponse in response.CheckInPassengersResponseData.CheckInMultiplePassengerResponseList.ToList()
                            from checkinResponse in checkInMultiplePassengersResponse.CheckInPaxResponseList.ToList()
                            where checkinResponse.ErrorList != null && checkinResponse.ErrorList.Length > 0
                            select checkinResponse.ErrorList.ToList();
            if (errorList != null && errorList.Count() > 0)
            {
                var errorMsgs = new List<string>();
                foreach (var err in errorList)
                    err.ForEach(m => errorMsgs.Add(m.ErrorMessage));
                throw new ResponseErrorException(dto.Enumerations.ResponseErrorCode.CheckInFailure, errorMsgs.ToArray());
            }

            return Mapper.Map<dto.CheckInPassengersResponse>(response);
        }

        public async Task<dto.GetBarCodedBoardingPassesResponse> GetBarCodedBoardingPasses(dto.GetBarCodedBoardingPassesRequest getBarCodedBoardingPassesRequest)
        {
            var booking = await _sessionBag.Booking();
            var boardingPassRequest = getBarCodedBoardingPassesRequest.BoardingPassRequest;
            var leg = booking.Journeys[boardingPassRequest.JourneyIndex].Segments[boardingPassRequest.SegmentIndex].Legs[boardingPassRequest.LegIndex];
            var pax = booking.Passengers[boardingPassRequest.PaxNumber];
            var request = new nskop.BoardingPassRequest
            {
                RecordLocator = booking.RecordLocator,
                InventoryLegKey = new Navitaire.WebServices.DataContracts.Common.InventoryLegKey
                {
                    ArrivalStation = leg.ArrivalStation,
                    DepartureStation = leg.DepartureStation,
                    CarrierCode = leg.FlightDesignator.CarrierCode,
                    FlightNumber = leg.FlightDesignator.FlightNumber,
                    OpSuffix = leg.FlightDesignator.OpSuffix,
                    DepartureDate = leg.STD
                },
                Name = new Navitaire.WebServices.DataContracts.Common.Name
                {
                    FirstName = pax.Names[0].FirstName,
                    LastName = pax.Names[0].LastName,
                    Title = pax.Names[0].Title
                },
                BarCodeType = Mapper.Map<BarCodeType>(getBarCodedBoardingPassesRequest.BoardingPassRequest.BarCodeType)
            };
            var response = await _client.GetBarCodedBoardingPassesAsync(new GetBarCodedBoardingPassesRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                BoardingPassRequest = request
            });
            var mappedResponse = Mapper.Map<dto.GetBarCodedBoardingPassesResponse>(response);
            mappedResponse.BarcodeHeight = boardingPassRequest.BarcodeHeight;
            mappedResponse.BarcodeWidth = boardingPassRequest.BarcodeWidth;
            return mappedResponse;
        }
        



        /*
        public async Task<object> BoardPassenger(int paxIndex, int journeyIndex, int segmentIndex)
        {
            var booking = await _sessionBag.Booking();
            var segment = booking.Journeys[journeyIndex].Segments[segmentIndex];
            var inventoryLeg = new Navitaire.WebServices.DataContracts.Common.InventoryLegKey
            {
                DepartureStation = segment.DepartureStation,
                ArrivalStation = segment.ArrivalStation,
                CarrierCode = segment.FlightDesignator.CarrierCode,
                FlightNumber = segment.FlightDesignator.FlightNumber,
                OpSuffix = segment.FlightDesignator.OpSuffix,
                DepartureDate = segment.STD
            };

            var boardResp = await _client.BoardPassengerAsync(new BoardPassengerRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                BoardPassengerRequestData = new nskop.BoardPassengerRequestData
                {
                    InventoryLegKey = inventoryLeg,
                    PassengerID = booking.Passengers[paxIndex].PassengerID,
                    RecordLocator = booking.RecordLocator,
                    SkipSecurityChecks = true,
                    Name = new Navitaire.WebServices.DataContracts.Common.Name
                    {
                        FirstName = booking.Passengers[paxIndex].Names[0].FirstName,
                        LastName = booking.Passengers[paxIndex].Names[0].LastName,
                        Title = booking.Passengers[paxIndex].Names[0].Title
                    }
                }
            });
            return boardResp;
        }
        */

        /* not working
        public async Task<object> ProcessBaggage(int paxIndex, int journeyIndex, int segmentIndex)
        {
            var booking = await _sessionBag.Booking();
            var segment = booking.Journeys[journeyIndex].Segments[segmentIndex];
            var legKey = string.Format("{0}{1}{2}{3}{4}{5}",
                        segment.STD.ToString("yyyyMMdd"), segment.FlightDesignator.CarrierCode.PadLeft(3, ' '), segment.FlightDesignator.FlightNumber.PadLeft(4, ' '),
                        segment.FlightDesignator.OpSuffix, segment.DepartureStation, segment.ArrivalStation);
            var paxSegment = segment.PaxSeats.ToList().Find(p => p.PassengerNumber == paxIndex);

            var request = new ProcessBaggageRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                ProcessBaggageRequestData = new Navitaire.WebServices.DataContracts.Booking.ProcessBaggageRequestData
                {
                    BaggageActionType = ProcessBaggageActionType.Add,
                    CollectedCurrencyCode = booking.CurrencyCode,
                     
                    PassengerBaggageRequestList = new Navitaire.WebServices.DataContracts.Booking.PassengerBaggageRequest[]
                    { 
                        new Navitaire.WebServices.DataContracts.Booking.PassengerBaggageRequest
                        { 
                            BaggageInfoList = new Navitaire.WebServices.DataContracts.Booking.BaggageInfo[]
                            {
                                new Navitaire.WebServices.DataContracts.Booking.BaggageInfo
                                {
                                    BaggageStatus = BaggageStatus.Added,
                                    TaggedToStation = segment.ArrivalStation,
                                    Weight = 19,
                                    OSTag = "12345",
                                    TaggedToCarrierCode = segment.FlightDesignator.CarrierCode,
                                    TaggedToFlightNumber = segment.FlightDesignator.FlightNumber,
                                    ManualBagTag = false,
                                    BaggageType = "Bag"
                                }
                            },
                            InventoryLegKey = legKey,
                            PassengerNumber = (short)paxIndex,
                            TaggedToStation = segment.ArrivalStation,
                              
                        }
                    }
                }
            };
            var processBaggageResponse = await _client.ProcessBaggageAsync(request);

            request.ProcessBaggageRequestData.BaggageActionType = ProcessBaggageActionType.CheckIn;
            request.ProcessBaggageRequestData.PassengerBaggageRequestList[0].BaggageInfoList[0].BaggageStatus = BaggageStatus.Checked;
            processBaggageResponse = await _client.ProcessBaggageAsync(request);

            return processBaggageResponse;
        }
        */
    }
}
