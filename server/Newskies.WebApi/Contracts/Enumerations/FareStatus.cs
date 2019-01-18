using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum FareStatus
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        SameDayStandBy = 1,

        [EnumMember]
        FareOverrideConfirming = 2,

        [EnumMember]
        FareOverrideConfirmed = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
