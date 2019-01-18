using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetAvailabilityResponse
    {
        [DataMember, ValidationInterceptor]
        public TripAvailabilityResponse GetTripAvailabilityResponse { get; set; }
    }


}
