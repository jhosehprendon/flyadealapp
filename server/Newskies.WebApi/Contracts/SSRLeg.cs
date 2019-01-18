using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SSRLeg
    {
        [DataMember, ValidationInterceptor]
        public LegKey LegKey { get; set; }

        [DataMember, ValidationInterceptor]
        public short Available { get; set; }
    }
}
