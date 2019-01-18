using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    public enum APPSTransitType
    {
        [EnumMember]
        TransitDefault = 0,

        [EnumMember]
        TransitAtOrigin = 1,

        [EnumMember]
        TransitAtDestination = 2,

        [EnumMember]
        TransitAtOriginAndDestination = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
