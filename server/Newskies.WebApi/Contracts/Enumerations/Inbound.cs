using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum InboundOutbound
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Inbound = 1,

        [EnumMember]
        Outbound = 2,

        [EnumMember]
        Both = 3,

        [EnumMember]
        RoundFrom = 4,

        [EnumMember]
        RoundTo = 5,

        [EnumMember]
        Unmapped = -1,
    }
}
