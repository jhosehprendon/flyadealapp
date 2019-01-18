using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SeatGroupPassengerFee
    {
        [DataMember, ValidationInterceptor]
        public short SeatGroup { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerFee PassengerFee { get; set; }

        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }
    }
}
