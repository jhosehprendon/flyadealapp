using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class CheckInMultiplePassengerRequestAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var requests = value as CheckInMultiplePassengerRequest[];
            if (requests == null)
                return new ValidationResult("Null object: " + typeof(CheckInMultiplePassengerRequest).Name);
            validationContext.GetSessionBagService();
            var sessionBagService = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
            if (booking == null)
            {
                return new ValidationResult("There is no booking in the session");
            }
            foreach (var req in requests)
            {
                var validationError = booking.ValidateJourneySegmentLegIndexes(req.JourneyIndex, req.SegmentIndex);
                if (validationError != null)
                    return validationError;

                if (req.PassengerNumbers == null || req.PassengerNumbers.Length == 0)
                    return new ValidationResult("Passenger number(s) required. ");
                foreach (var paxNum in req.PassengerNumbers)
                    if (booking.Passengers.ToList().Find(p => p.PassengerNumber == paxNum) == null)
                        return new ValidationResult("Invalid passenger number: " + paxNum);
            }

            return ValidationResult.Success;
        }
    }
}
