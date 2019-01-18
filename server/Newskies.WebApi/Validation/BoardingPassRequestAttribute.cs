using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class BoardingPassRequestAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var boardingPassRequest = value as BoardingPassRequest;
            if (boardingPassRequest == null)
                return new ValidationResult("Object must be of type " + typeof(BoardingPassRequest).Name);
            var sessionBagService = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
            var validationError = booking.ValidateJourneySegmentLegIndexes(boardingPassRequest.JourneyIndex, boardingPassRequest.SegmentIndex, boardingPassRequest.LegIndex);
            if (validationError != null)
                return validationError;
            if (!booking.Passengers.ToList().Exists(p => p.PassengerNumber == boardingPassRequest.PaxNumber))
                return new ValidationResult(string.Format("PaxNumber {0} is invalid.", boardingPassRequest.PaxNumber));
            if (!booking.Journeys[boardingPassRequest.JourneyIndex].Segments[boardingPassRequest.SegmentIndex].IsPaxCheckedIn(boardingPassRequest.PaxNumber))
                return new ValidationResult(string.Format("Pax {0} is not checked in for this segment.", boardingPassRequest.PaxNumber));
            return ValidationResult.Success;
        }
    }
}
