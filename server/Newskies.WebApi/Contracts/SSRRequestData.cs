using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SSRRequestData
    {
        [DataMember, StringLength(10, MinimumLength = 2), ValidationInterceptor]
        public string SSRCode { get; set; }

        [DataMember, StringLength(128), ValidationInterceptor]
        public string Note { get; set; }

        [DataMember, ValidationInterceptor]
        public int PaxNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public int SSRCount { get; set; }

        [DataMember, ValidationInterceptor]
        public int JourneyIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int SegmentIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int? LegIndex { get; set; }
    }
}
