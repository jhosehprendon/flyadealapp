using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SeatAvailabilityRequest
    {
        [DataMember, SeatAvailability, ValidationInterceptor]
        public SeatAvailabilityRequestData SeatAvailabilityRequestData { get; set; }
    }
}
