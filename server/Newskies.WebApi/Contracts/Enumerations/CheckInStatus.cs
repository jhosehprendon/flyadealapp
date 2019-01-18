using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum CheckInStatus
    {
        [EnumMember]
        NotYetOpen = 0,

        [EnumMember]
        Open = 1,

        [EnumMember]
        Closed = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
