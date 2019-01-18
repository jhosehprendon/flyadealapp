using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum ClassStatus
    {
        [EnumMember]
        Active = 0,
        [EnumMember]
        AVSClosed = 4,
        [EnumMember]
        AVSOnRequest = 3,
        [EnumMember]
        AVSOpen = 2,
        [EnumMember]
        InActive = 1
    }
}
