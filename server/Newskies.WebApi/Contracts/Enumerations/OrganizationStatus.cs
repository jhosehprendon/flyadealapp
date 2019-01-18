using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum OrganizationStatus
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Active = 1,

        [EnumMember]
        Cancelled = 2,

        [EnumMember]
        Pending = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
