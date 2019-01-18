using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetSeatAvailabilityResponse
    {
        [DataMember, ValidationInterceptor]
        public SeatAvailabilityResponse SeatAvailabilityResponse { get; set; }
    }
}
