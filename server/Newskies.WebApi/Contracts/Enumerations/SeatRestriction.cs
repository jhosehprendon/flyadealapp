using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum SeatRestriction
    {
        [EnumMember]
        Undefined = 0,

        [EnumMember]
        AlwaysAllowed = 1,

        [EnumMember]
        DefaultAllowed = 2,

        [EnumMember]
        DefaultRestricted = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
