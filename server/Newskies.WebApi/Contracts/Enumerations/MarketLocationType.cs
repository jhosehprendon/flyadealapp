using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum MarketLocationType
    {
        [EnumMember]
        Undefined = 0,

        [EnumMember]
        Station = 1,

        [EnumMember]
        MAC = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
