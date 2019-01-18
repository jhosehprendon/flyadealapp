using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CompartmentInfo
    {
        [DataMember, ValidationInterceptor]
        public string CompartmentDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public short Deck { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int Length { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int Width { get; set; }

        [DataMember, ValidationInterceptor]
        public int AvailableUnits { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short Orientation { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short Sequence { get; set; }

        [DataMember, ValidationInterceptor]
        public SeatInfo[] Seats { get; set; }

        //[DataMember, ValidationInterceptor]
        //public EquipmentProperty[] PropertyList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public uint[] PropertyBits { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int[] PropertyInts { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime PropertyTimestamp { get; set; }
    }
}
