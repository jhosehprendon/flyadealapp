using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetSSRAvailabilityForBookingResponse
    {
        [DataMember, ValidationInterceptor]
        public SSRAvailabilityForBookingResponse SSRAvailabilityForBookingResponse { get; set; }
    }
}
