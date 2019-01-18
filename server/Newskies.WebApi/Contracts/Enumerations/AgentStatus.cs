using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum AgentStatus
    {
        [EnumMember]
        Active = 1,

        [EnumMember]
        Default = 0,

        [EnumMember]
        Pending = 4,

        [EnumMember]
        Suspended = 3,

        [EnumMember]
        Terminated = 2
    }
}
