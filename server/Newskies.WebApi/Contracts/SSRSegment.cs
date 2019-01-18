using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SSRSegment
    {
        [DataMember, ValidationInterceptor]
        public LegKey LegKey { get; set; }

        [DataMember, ValidationInterceptor]
        public AvailablePaxSSR[] AvailablePaxSSRList { get; set; }
    }
}
