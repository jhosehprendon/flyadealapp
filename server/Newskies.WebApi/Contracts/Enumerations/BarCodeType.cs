using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum BarCodeType
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Airport = 1,

        [EnumMember]
        M2D = 2,

        [EnumMember]
        S2D = 3,

        [EnumMember]
        Type1_1D = 4,

        [EnumMember]
        Type2_1D = 5,

        [EnumMember]
        Type3_1D = 6,

        [EnumMember]
        Type4_1D = 7,

        [EnumMember]
        BothType1 = 8,

        [EnumMember]
        BothType2 = 9,

        [EnumMember]
        BothType3 = 10,

        [EnumMember]
        BothType4 = 11,

        [EnumMember]
        Type3_1D_Plus_Space = 12,

        [EnumMember]
        Type3_1D_Date = 13,

        [EnumMember]
        BothType3_Plus_Space = 14,

        [EnumMember]
        BothType3_Date = 15,

        [EnumMember]
        Type5_1D = 16,

        [EnumMember]
        BothType5 = 17,

        [EnumMember]
        Unmapped = -1,
    }
}
