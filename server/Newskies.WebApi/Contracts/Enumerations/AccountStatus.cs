using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum AccountStatus
    {

        [EnumMember]
        Default = 0,

        [EnumMember]
        Open = 1,

        [EnumMember]
        Closed = 2,

        [EnumMember]
        AgencyInactive = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
