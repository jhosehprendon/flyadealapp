using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingRetrieveInfo
    {
        [DataMember, ValidationInterceptor]
        public bool HasBalanceDue { get; set; }

        [DataMember, ValidationInterceptor]
        public bool AllPaxSegmentsCheckedIn { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxJourneySegmentInfo[] PaxJourneySegmentInfos { get; set; }
    }
}