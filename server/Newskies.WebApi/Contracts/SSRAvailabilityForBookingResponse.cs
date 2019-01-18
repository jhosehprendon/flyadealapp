using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SSRAvailabilityForBookingResponse
    {
        [DataMember, ValidationInterceptor]
        public SSRSegment[] SSRSegmentList { get; set; }
    }
}
