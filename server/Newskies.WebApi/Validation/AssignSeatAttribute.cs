using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class AssignSeatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var assignSeatData = value as AssignSeatData;
            if (assignSeatData == null)
                return new ValidationResult("Null object: " + typeof(AssignSeatData).Name);
            var sessionBagService = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
            if (booking == null)
            {
                return new ValidationResult("There is no booking in the session");
            }
            var validationError = booking.ValidateJourneySegmentLegIndexes(assignSeatData.JourneyIndex, assignSeatData.SegmentIndex, assignSeatData.LegIndex);
            if (validationError != null)
                return validationError;

            if (!assignSeatData.PaxNumber.HasValue && string.IsNullOrEmpty(assignSeatData.CompartmentDesignator) && string.IsNullOrEmpty(assignSeatData.UnitDesignator))
                return ValidationResult.Success; // auto-assign seat(s) is requested
            if (!assignSeatData.PaxNumber.HasValue || assignSeatData.PaxNumber.Value >= booking.Passengers.Length)
                return new ValidationResult("Invalid pax number.");
            return ValidationResult.Success;
        }
    }
}
