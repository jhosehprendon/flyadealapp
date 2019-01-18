using System.Runtime.Serialization;
namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public enum PaxSegmentCheckInStatus
    {
        [EnumMember]
        BookingNotComplete = 0,

        [EnumMember]
        CheckInNotYetOpen = 1,

        [EnumMember]
        TooCloseToDeparture = 2,

        [EnumMember]
        AlreadyCheckedIn = 3,

        [EnumMember]
        FlightHasAlreadyDeparted = 4,

        [EnumMember]
        PaxHasInfant = 5,

        [EnumMember]
        RestrictedByAirport = 6,

        [EnumMember]
        Allowed = 7,
    }
}