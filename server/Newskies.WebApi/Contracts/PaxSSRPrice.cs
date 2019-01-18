using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxSSRPrice
    {
        [DataMember, ValidationInterceptor]
        public PassengerFee PaxFee { get; set; }

        [DataMember, ValidationInterceptor]
        public short[] PassengerNumberList { get; set; }
    }
}
