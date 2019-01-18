using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum OrganizationType
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Master = 1,

        [EnumMember]
        Carrier = 2,

        [EnumMember]
        TravelAgency = 3,

        [EnumMember]
        ThirdParty = 4,

        [EnumMember]
        Unmapped = -1,
    }
}
