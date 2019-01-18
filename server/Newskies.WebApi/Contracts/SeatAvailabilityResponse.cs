using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SeatAvailabilityResponse
    {
        [DataMember, ValidationInterceptor]
        public EquipmentInfo[] EquipmentInfos { get; set; }

        [DataMember, ValidationInterceptor]
        public SeatGroupPassengerFee[] SeatGroupPassengerFees { get; set; }

        [DataMember, ValidationInterceptor]
        public SeatAvailabilityLeg[] Legs { get; set; }

        [DataMember, ValidationInterceptor]
        public EquipmentPropertyTypeCodesLookup PropertyTypeCodesLookup { get; set; }
    }
}
