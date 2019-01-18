using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum PriceStatus
    {
        [EnumMember]
        Invalid = 0,

        [EnumMember]
        Override = 1,

        [EnumMember]
        Valid = 3
    }
}
