using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum SeatAvailability
    {
        [EnumMember]
        Blocked = 2,

        [EnumMember]
        Broken = 10,

        [EnumMember]
        CheckedIn = 7,

        [EnumMember]
        FleetBlocked = 8,

        [EnumMember]
        HeldForAnotherSession = 3,

        [EnumMember]
        HeldForThisSession = 4,

        [EnumMember]
        Missing = 6,

        [EnumMember]
        Open = 5,

        [EnumMember]
        Reserved = 1,

        [EnumMember]
        ReservedForPNR = 11,

        [EnumMember]
        Restricted = 9,

        [EnumMember]
        SoftBlocked = 12,

        [EnumMember]
        Unavailable = 13,

        [EnumMember]
        Unknown = 0
    }
}
