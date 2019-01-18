using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class EquipmentInfo
    {
        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string EquipmentType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string EquipmentTypeSuffix { get; set; }

        [DataMember, ValidationInterceptor]
        public int AvailableUnits { get; set; }

        [DataMember, ValidationInterceptor]
        public CompartmentInfo[] Compartments{ get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.EquipmentCategory EquipmentCategory{ get; set; }

        //[DataMember, ValidationInterceptor]
        //public EquipmentProperty[] PropertyList{ get; set; }

        //[DataMember, ValidationInterceptor]
        //public uint[] PropertyBits { get; set; }

        //[DataMember, ValidationInterceptor]
        //public int[] PropertyInts { get; set; }

        //[DataMember, ValidationInterceptor]
        //public uint[] PropertyBitsInUse { get; set; }

        //[DataMember, ValidationInterceptor]
        //public uint[] PropertyIntsInUse { get; set; }

        //[DataMember, ValidationInterceptor]
        //public uint[] SSRBitsInUse { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string MarketingCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string Name { get; set; }
    }
}
