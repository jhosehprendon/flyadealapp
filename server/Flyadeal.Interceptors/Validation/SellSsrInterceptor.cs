using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System;
using Flyadeal.Interceptors.Helpers;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    class SellSsrInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext context)
        {
            var request = value as SSRRequestData;
            if (request == null)
                return new ValidationResult("Failed to validate: null ssr request");
            var sessionBagService = context.GetSessionBagService();
            var booking = Task.Run(async() => await sessionBagService.Booking()).Result;
            if (booking == null)
                return new ValidationResult("Unable to validate ssr request. No booking found in session.");
            booking.PopulateCheckInInformation();
            if (booking.Journeys[request.JourneyIndex].Segments[request.SegmentIndex].ChangesNotAllowed())
            {
                return new ValidationResult("Changes to the segment are not allowed at this time. ");
            }
            if (request.SSRCode.EndsWith("ML"))
            {
                DateTime currTime = DateTime.Now;
                DateTime cutOffTime = booking.Journeys[request.JourneyIndex].Segments[0].STD.AddDays(-1);
                cutOffTime = cutOffTime.Date + new TimeSpan(18, 0, 0);
                if (currTime > cutOffTime)
                    return new ValidationResult("Meals can not be added after 6pm the day before departure");
            }
            if (request.SSRCode == "WCHR" && !Helpers.ValidationHelper.SpecialAssistanceSSRSellAllowed(booking))
            {
                return new ValidationResult("Special Assistance can not be added within 24 hours of departure of initial flight. ");
            }
            return ValidationResult.Success;
        }
    }
}
