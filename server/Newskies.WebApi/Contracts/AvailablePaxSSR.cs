using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AvailablePaxSSR
    {
        [DataMember, ValidationInterceptor]
        public string SSRCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InventoryControlled { get; set; }

        [DataMember, ValidationInterceptor]
        public bool NonInventoryControlled { get; set; }

        [DataMember, ValidationInterceptor]
        public bool SeatDependent { get; set; }

        [DataMember, ValidationInterceptor]
        public bool NonSeatDependent { get; set; }

        [DataMember, ValidationInterceptor]
        public short Available { get; set; }

        [DataMember, ValidationInterceptor]
        public short[] PassengerNumberList { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxSSRPrice[] PaxSSRPriceList { get; set; }

        [DataMember, ValidationInterceptor]
        public SSRLeg[] SSRLegList { get; set; }
    }
}
