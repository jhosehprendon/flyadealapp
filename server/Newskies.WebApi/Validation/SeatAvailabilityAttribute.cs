using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class SeatAvailabilityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var seatAvailabilityRequestData = value as SeatAvailabilityRequestData;
            if (seatAvailabilityRequestData == null)
                return new ValidationResult("Object must be of type " + typeof(SeatAvailabilityRequestData).Name);
            var sessionBagService = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
            var validationError = booking.ValidateJourneySegmentLegIndexes(seatAvailabilityRequestData.JourneyIndex, seatAvailabilityRequestData.SegmentIndex, seatAvailabilityRequestData.LegIndex);
            if (validationError != null)
                return validationError;
            return ValidationResult.Success;
        }
    }
}
