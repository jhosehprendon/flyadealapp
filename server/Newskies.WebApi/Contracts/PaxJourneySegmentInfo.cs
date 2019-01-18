using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    public class PaxJourneySegmentInfo
    {
        [DataMember, ValidationInterceptor]
        public int JourneyIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int SegmentIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public CheckInStatus CheckInStatus { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxLiftStatus[] PaxLiftStatuses { get; set; }
    }
}