using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SSR
    {
        [DataMember, ValidationInterceptor]
        public bool Allowed { get; set; }

        [DataMember, ValidationInterceptor]
        public short BoardingZone { get; set; }

        [DataMember, ValidationInterceptor]
        public string FeeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }

        [DataMember, ValidationInterceptor]
        public short LimitPerPassenger { get; set; }

        [DataMember, ValidationInterceptor]
        public string Name { get; set; }

        [DataMember, ValidationInterceptor]
        public string RuleSetName { get; set; }

        [DataMember, ValidationInterceptor]
        public string SeatMapCode { get; set; }

        [DataMember, ValidationInterceptor]
        public SeatRestriction SeatRestriction { get; set; }

        [DataMember, ValidationInterceptor]
        public string SSRCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string SSRNestCode { get; set; }

        [DataMember, ValidationInterceptor]
        public SSRType SSRType { get; set; }

        [DataMember, ValidationInterceptor]
        public string TraceQueueCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short UnitValue { get; set; }
    }
}
