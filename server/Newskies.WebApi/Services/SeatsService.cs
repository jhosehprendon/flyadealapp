using AutoMapper;
using System;
using System.Threading.Tasks;
using Navitaire.WebServices.DataContracts.Booking;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using dto = Newskies.WebApi.Contracts;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System.Collections.Generic;
using System.Linq;
using Newskies.WebApi.Helpers;

namespace Newskies.WebApi.Services
{
    public interface ISeatsService
    {
        Task<dto.GetSeatAvailabilityResponse> GetSeatAvailability(dto.SeatAvailabilityRequest request);
        Task<dto.AssignSeatsResponse> AssignSeat(dto.AssignSeatRequest assignSeatRequest);
        Task<dto.AssignSeatsResponse> UnAssignSeat(dto.AssignSeatRequest assignSeatRequest);
    }

    public class SeatsService : ServiceBase, ISeatsService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IBookingService _bookingService;
        private readonly IBookingManager _client;

        public SeatsService(ISessionBagService sessionBag, IBookingService bookingService,
            IBookingManager client, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<dto.GetSeatAvailabilityResponse> GetSeatAvailability(dto.SeatAvailabilityRequest request)
        {
            var booking = await _sessionBag.Booking();
            var journeyIndex = request.SeatAvailabilityRequestData.JourneyIndex;
            var segmentIndex = request.SeatAvailabilityRequestData.SegmentIndex;
            var legIndex = request.SeatAvailabilityRequestData.SegmentIndex;
            var leg = booking.Journeys[journeyIndex].Segments[segmentIndex].Legs[legIndex];
            var seatAvailabilityRequest = new SeatAvailabilityRequest
            {
                DepartureStation = leg.DepartureStation,
                ArrivalStation = leg.ArrivalStation,
                CarrierCode = leg.FlightDesignator.CarrierCode,
                FlightNumber = leg.FlightDesignator.FlightNumber,
                OpSuffix = leg.FlightDesignator.OpSuffix,
                IncludePropertyLookup = true,
                IncludeSeatFees = true,
                STD = leg.STD,
                SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment,
                CollectedCurrencyCode = booking.CurrencyCode,
                CompressProperties = true,
                SeatAvailabilityFilter = new SeatAvailabilityFilter
                {
                    ExcludeFacilities = true
                }
            };
            var seatAvailabilityResponse = await _client.GetSeatAvailabilityAsync(new GetSeatAvailabilityRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                SeatAvailabilityRequest = seatAvailabilityRequest
            });
            var mappedResponse = Mapper.Map<dto.GetSeatAvailabilityResponse>(seatAvailabilityResponse);
            await _sessionBag.SetSeatAvailabilityResponse(mappedResponse, journeyIndex, segmentIndex, legIndex);
            return mappedResponse;
        }

        public async Task<dto.AssignSeatsResponse> AssignSeat(dto.AssignSeatRequest assignSeatRequest)
        {
            var sellSeatRequest = await GetSellSeatRequest(assignSeatRequest, false);
            var assignSeatsRequest = new AssignSeatsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                SellSeatRequest = sellSeatRequest
            };
            var assignSeatsResponse = await _client.AssignSeatsAsync(assignSeatsRequest);
            var error = NewskiesHelper.GetUpdateResponseError(assignSeatsResponse.BookingUpdateResponseData);
            if (error != null)
            {
                throw new dto.ResponseErrorException(
                    dto.Enumerations.ResponseErrorCode.SeatAssignmentFailure, error);
            }
            return Mapper.Map<dto.AssignSeatsResponse>(assignSeatsResponse);
        }

        public async Task<dto.AssignSeatsResponse> UnAssignSeat(dto.AssignSeatRequest assignSeatRequest)
        {
            var sellSeatRequest = await GetSellSeatRequest(assignSeatRequest, true);
            var unAssignSeatsResponse = await _client.UnassignSeatsAsync(new UnassignSeatsRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBag.Signature(),
                EnableExceptionStackTrace = false,
                SellSeatRequest = sellSeatRequest
            });
            var error = NewskiesHelper.GetUpdateResponseError(unAssignSeatsResponse.BookingUpdateResponseData);
            if (error != null)
            {
                throw new dto.ResponseErrorException(
                    dto.Enumerations.ResponseErrorCode.SeatAssignmentFailure, error);
            }
            return Mapper.Map<dto.AssignSeatsResponse>(unAssignSeatsResponse);
        }

        private async Task<SeatSellRequest> GetSellSeatRequest(dto.AssignSeatRequest assignSeatRequest, bool isUnassign)
        {
            var booking = await _sessionBag.Booking();
            var journeyIndex = assignSeatRequest.AssignSeatData.JourneyIndex;
            var segmentIndex = assignSeatRequest.AssignSeatData.SegmentIndex;
            var legIndex = assignSeatRequest.AssignSeatData.SegmentIndex;
            var paxNumber = assignSeatRequest.AssignSeatData.PaxNumber;
            var mode = assignSeatRequest.AssignSeatData.SeatAssignmentMode;
            var waiveFees = assignSeatRequest.AssignSeatData.WaiveFees;
            var compartmentDesignator = assignSeatRequest.AssignSeatData.CompartmentDesignator;
            var unitDesignator = assignSeatRequest.AssignSeatData.UnitDesignator;
            var leg = Mapper.Map<Leg>(booking.Journeys[journeyIndex].Segments[segmentIndex].Legs[legIndex]);
            var paxNumbers = GetPaxNumbers(booking, leg, journeyIndex, segmentIndex, paxNumber, isUnassign);
            var seatSellRequest = new SeatSellRequest
            {
                SeatAssignmentMode = Mapper.Map<SeatAssignmentMode>(mode),
                CollectedCurrencyCode = booking.CurrencyCode,
                WaiveFee = waiveFees,
                AllowSeatSwappingInPNR = true,
                SegmentSeatRequests = new SegmentSeatRequest[]
                {
                    new SegmentSeatRequest
                    {
                        DepartureStation = leg.DepartureStation,
                        ArrivalStation = leg.ArrivalStation,
                        FlightDesignator = leg.FlightDesignator,
                        STD = leg.STD,
                        PassengerNumbers = paxNumbers,
                        CompartmentDesignator = compartmentDesignator,
                        UnitDesignator = unitDesignator
                    }
                }
            };
            return seatSellRequest;
        }

        private short[] GetPaxNumbers(dto.Booking booking, Leg leg, int journeyIndex, int segmentIndex, short? paxNumber, bool isUnassign)
        {
            var paxNumbers = new List<short>();
            if (!paxNumber.HasValue)
            {
                // Auto-assign seats for paxes who don't yet have a seat -OR- Auto-unassign paxes who have a seat and not already checked in and not yet flown.
                var segmentPaxSeats = booking.Journeys[journeyIndex].Segments[segmentIndex].PaxSeats;
                var paxSegments = booking.Journeys[journeyIndex].Segments[segmentIndex].PaxSegments;
                foreach (var pax in booking.Passengers)
                {
                    var paxSegment = paxSegments.ToList().Find(p => p.PassengerNumber == pax.PassengerNumber);
                    if (paxSegment.LiftStatus == dto.Enumerations.LiftStatus.Default)
                    {
                        var paxSeat = segmentPaxSeats.ToList().Find(p => p.PassengerNumber == pax.PassengerNumber && p.DepartureStation == leg.DepartureStation && p.ArrivalStation == leg.ArrivalStation);
                        if (!isUnassign ? paxSeat == null : paxSeat != null)
                        {
                            paxNumbers.Add(pax.PassengerNumber);
                        }
                    }
                }
                if (paxNumbers.Count() == 0)
                    throw !isUnassign ? new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.AllSeatsAlreadyAssigned, "All paxes already have assigned seats. ")
                        : new dto.ResponseErrorException(dto.Enumerations.ResponseErrorCode.NoSeatsToUnAssign, "No paxes have seats to unassign. ");

            }
            else
            {
                paxNumbers.Add(paxNumber.Value);
            }
            return paxNumbers.ToArray();
        }
    }
}
