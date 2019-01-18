using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Services;
using System.Linq;
using Newskies.WebApi.Constants;
using System;
using Flyadeal.Interceptors.Helpers;
using Newskies.WebApi.Helpers;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class AssignSeatRequestInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var data = value as AssignSeatData;
            var sessionBag = validationContext.GetSessionBagService();
            if (data == null || sessionBag == null)
                return new ValidationResult("General validation failure for " + validationContext.DisplayName);

            var bookingService = validationContext.GetService(typeof(IBookingService)) as BookingService;
            var booking = Task.Run(async () => await bookingService.GetSessionBooking()).Result;
            if (booking == null)
                return new ValidationResult("Unable to validate seat request. No booking found in session. ");
            booking.PopulateCheckInInformation();
            var validationErr = booking.ValidateJourneySegmentLegIndexes(data.JourneyIndex, data.SegmentIndex, data.LegIndex);
            if (validationErr != null)
                return validationErr;
            if (booking.Journeys[data.JourneyIndex].Segments[data.SegmentIndex].ChangesNotAllowed())
                return new ValidationResult("Changes to the segment are not allowed at this time. ");
            var seatAvailability = sessionBag.SeatAvailabilityResponse(data.JourneyIndex, data.SegmentIndex, data.LegIndex).Result
                    ?? GetSeatAvailability(validationContext, data);
            if (seatAvailability.SeatAvailabilityResponse.EquipmentInfos == null
                || seatAvailability.SeatAvailabilityResponse.EquipmentInfos.Length < data.JourneyIndex
                || seatAvailability.SeatAvailabilityResponse.EquipmentInfos[0].Compartments == null
                || seatAvailability.SeatAvailabilityResponse.EquipmentInfos[0].Compartments.Length == 0
                || seatAvailability.SeatAvailabilityResponse.EquipmentInfos[0].Compartments[0].Seats == null)
                return new ValidationResult("Unable to validate seat request. SeatAvailabilityResponse contains no seatmap information. ");

            if (data.PaxNumber.HasValue) // assign single pax seat requested
            {
                var pax = booking.Passengers.ToList().Find(p => p.PassengerNumber == data.PaxNumber.Value);
                if (string.IsNullOrEmpty(data.CompartmentDesignator))
                    return new ValidationResult("CompartmentDesignator cannot be null. ");

                var compartment = seatAvailability.SeatAvailabilityResponse.EquipmentInfos[0].Compartments.ToList()
                    .Find(c => c.CompartmentDesignator.Equals(data.CompartmentDesignator, StringComparison.InvariantCultureIgnoreCase));
                if (compartment == null)
                    return new ValidationResult("Invalid CompartmentDesignator. ");
                if (string.IsNullOrEmpty(data.UnitDesignator))
                    return new ValidationResult("UnitDesignator cannot be null. ");

                var seat = compartment.Seats.ToList()
                    .Find(s => s.SeatDesignator.Equals(data.UnitDesignator, StringComparison.InvariantCultureIgnoreCase));
                if (seat == null)
                    return new ValidationResult("Invalid UnitDesignator. ");

                // children are not allowed on Premium seats
                if (pax.PassengerTypeInfo.PaxType == Global.CHILD_CODE && seat.SeatGroup == 1)
                    return new ValidationResult("Child not allowed on this seat. ");

                // adults aged 12 to 15 are not allowed on Premium and Exit seats. 1 is Premium, 4 is Exit
                if (pax.PassengerTypeInfo.PaxType == Global.ADULT_CODE && (seat.SeatGroup == 1 || seat.SeatGroup == 4))
                {
                    var departDate = booking.Journeys[data.JourneyIndex].Segments[data.SegmentIndex].STD;
                    var paxAge = Helpers.ValidationHelper.GetAge(departDate, pax.PassengerTypeInfo.DOB);
                    if (paxAge >= 12 && paxAge <= 15)
                        return new ValidationResult("Adult aged 12-15 not allowed on this seat. ");
                }
            }
            else  // auto-assign pax seats requested (is only allowed for WCI)
            {
                var wciAllowed = booking.Journeys[data.JourneyIndex].Segments[data.SegmentIndex].IsWebCheckInAllowed();
                if (string.IsNullOrEmpty(booking.RecordLocator) || !wciAllowed)
                    return new ValidationResult("Auto-assigning of seats disallowed. ");
            }
            return ValidationResult.Success;
        }

        internal Newskies.WebApi.Contracts.GetSeatAvailabilityResponse GetSeatAvailability(ValidationContext validationContext, AssignSeatData assignSeatData)
        {
            var seatsService = validationContext.GetService(typeof(ISeatsService)) as SeatsService;
            var response = seatsService.GetSeatAvailability(new SeatAvailabilityRequest
            {
                SeatAvailabilityRequestData = new SeatAvailabilityRequestData
                {
                    JourneyIndex = assignSeatData.JourneyIndex,
                    SegmentIndex = assignSeatData.SegmentIndex,
                    LegIndex = assignSeatData.LegIndex
                }
            }).Result;
            return response;
        }
    }
}
