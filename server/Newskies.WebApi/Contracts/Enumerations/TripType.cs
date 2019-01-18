using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum TripType
    {
        [EnumMember]
        All = 15,
        [EnumMember]
        CircleTrip = 8,
        [EnumMember]
        HalfRound = 3,
        [EnumMember]
        None = 0,
        [EnumMember]
        OneWay = 1,
        [EnumMember]
        OpenJaw = 4,
        [EnumMember]
        RoundTrip = 2
    }
}
