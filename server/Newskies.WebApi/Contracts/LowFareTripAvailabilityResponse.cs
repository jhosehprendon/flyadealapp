using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LowFareTripAvailabilityResponse
    {
        [DataMember, ValidationInterceptor]
        public LowFareAvailabilityResponse[] LowFareAvailabilityResponseList { get; set; }
    }
}
