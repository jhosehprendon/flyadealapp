using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum BookingStatus
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Hold = 1,

        [EnumMember]
        Confirmed = 2,

        [EnumMember]
        Closed = 3,

        [EnumMember]
        HoldCanceled = 4,

        [EnumMember]
        PendingArchive = 5,

        [EnumMember]
        Archived = 6,

        [EnumMember]
        Unmapped = -1,
    }
}
