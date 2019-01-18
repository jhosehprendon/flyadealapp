using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum PaidStatus
    {
        [EnumMember]
        OverPaid = 2,

        [EnumMember]
        PaidInFull = 1,

        [EnumMember]
        UnderPaid = 0
    }
}
