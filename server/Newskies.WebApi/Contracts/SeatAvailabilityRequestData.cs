using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SeatAvailabilityRequestData
    {
        [DataMember, ValidationInterceptor]
        public int JourneyIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int SegmentIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int LegIndex { get; set; }
    }
}
