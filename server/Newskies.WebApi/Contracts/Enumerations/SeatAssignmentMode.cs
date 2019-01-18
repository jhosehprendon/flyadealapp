using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum SeatAssignmentMode
    {
        [EnumMember]
        AutoDetermine = 0,

        [EnumMember]
        PreSeatAssignment = 1,

        [EnumMember]
        WebCheckIn = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
