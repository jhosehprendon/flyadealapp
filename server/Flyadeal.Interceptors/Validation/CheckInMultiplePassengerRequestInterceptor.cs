using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Services;
using System.Linq;
using Newskies.WebApi.Helpers;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class CheckInMultiplePassengerRequestInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sessionBag = validationContext.GetSessionBagService();
            var requests = value as CheckInMultiplePassengerRequest[];
            var booking = Task.Run(async () => await sessionBag.Booking()).Result;
            if (requests != null && booking != null)
            {
                foreach (var req in requests)
                {
                    var error = booking.ValidateJourneySegmentLegIndexes(req.JourneyIndex, req.SegmentIndex);
                    if (error != null)
                        continue;
                    var segment = booking.Journeys[req.JourneyIndex].Segments[req.SegmentIndex];
                    foreach (var paxNum in req.PassengerNumbers)
                    {
                        var paxSegment = segment.PaxSegments.ToList().Find(p => p.PassengerNumber == paxNum);
                        if (paxSegment != null && paxSegment.CheckInStatus.HasValue && paxSegment.CheckInStatus.Value != PaxSegmentCheckInStatus.Allowed)
                            return new ValidationResult(string.Format("Check in not allowed for segment {0}-{1}. Reason: {2}",
                                segment.DepartureStation, segment.ArrivalStation, paxSegment.CheckInStatus.Value));
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
