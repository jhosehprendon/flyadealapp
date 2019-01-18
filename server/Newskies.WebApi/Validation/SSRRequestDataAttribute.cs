using Newskies.WebApi.Constants;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class SSRRequestDataAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var data = value as SSRRequestData;
            if (data == null || data == null)
                return new ValidationResult(string.Format("{0} is missing.", validationContext.DisplayName));
            var sessionBagService = validationContext.GetSessionBagService();
            try
            {
                // disallow infant SSR
                if (data.SSRCode == Global.INFANT_CODE)
                    return new ValidationResult(string.Format("{0} SSR cannot be sold here.", data.SSRCode));

                // validate SSR code
                var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
                var response = Task.Run(async () => await resourcesService.GetSSRList()).Result;
                var ssr = Array.Find(response.SSRList, c => c.SSRCode.Equals(data.SSRCode));
                if (ssr == null)
                    return new ValidationResult(string.Format("Invalid SSR code: {0}.", data.SSRCode));

                // validate booking indexes
                var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
                if (data.JourneyIndex >= booking.Journeys.Length)
                    return new ValidationResult(string.Format("{0} journeyIndex {1} out of bounds.", validationContext.DisplayName, data.JourneyIndex));
                if (data.SegmentIndex >= booking.Journeys[data.JourneyIndex].Segments.Length)
                    return new ValidationResult(string.Format("{0} segmentIndex {1} out of bounds.", validationContext.DisplayName, data.SegmentIndex));
                if (data.LegIndex.HasValue && data.LegIndex.Value >= booking.Journeys[data.JourneyIndex].Segments[data.SegmentIndex].Legs.Length)
                    return new ValidationResult(string.Format("{0} legIndex {1} out of bounds.", validationContext.DisplayName, data.LegIndex.Value));

                // validate pax number
                var pax = Array.Find(booking.Passengers, p => p.PassengerNumber == data.PaxNumber);
                if (pax == null)
                    return new ValidationResult(string.Format("{0} passengerNumber {1} is invalid.", validationContext.DisplayName, data.PaxNumber));

                // validation passed
                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult(string.Format("Unable to validate {0}. {1}", validationContext.DisplayName, e.Message));
            }
        }
    }
}
