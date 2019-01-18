using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SeatInfo
    {
        //[DataMember, ValidationInterceptor]
        //public bool Assignable { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int CabotageLevel { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int CarAvailableUnits { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CompartmentDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public int SeatSet { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int CriterionWeight { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int SeatSetAvailableUnits { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SSRSeatMapCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short SeatAngle { get; set; }

        [DataMember, ValidationInterceptor]
        public SeatAvailability SeatAvailability { get; set; }

        [DataMember, ValidationInterceptor]
        public string SeatDesignator { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SeatType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short X { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short Y { get; set; }

        //[DataMember, ValidationInterceptor]
        //public EquipmentProperty[] PropertyList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string[] SSRPermissions { get; set; }

        //[DataMember, ValidationInterceptor]
        //public uint[] SSRPermissionBits { get; set; }

        [DataMember, ValidationInterceptor]
        public uint[] PropertyBits { get; set; }

        [DataMember, ValidationInterceptor]
        public int[] PropertyInts { get; set; }

        [DataMember, ValidationInterceptor]
        public string TravelClassCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime PropertyTimestamp { get; set; }

        [DataMember, ValidationInterceptor]
        public short SeatGroup { get; set; }

        [DataMember, ValidationInterceptor]
        public short Zone { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short Height { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short Width { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short Priority { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string Text { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int ODPenalty { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string TerminalDisplayCharacter { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool PremiumSeatIndicator { get; set; }
    }
}
