using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BarCodedBoardingPassLeg
    {
        [DataMember, ValidationInterceptor]
        public short BoardingZone { get; set; }
        [DataMember, ValidationInterceptor]
        public short BoardingSequence { get; set; }
        [DataMember, ValidationInterceptor]
        public short SeatRow { get; set; }
        [DataMember, ValidationInterceptor]
        public string SeatColumn { get; set; }
        //[DataMember, ValidationInterceptor]
        //public InventoryLegKey InventoryLegKey { get; set; }
    }
}