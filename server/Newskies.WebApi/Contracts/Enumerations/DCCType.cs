using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum DCCType
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        ZeroRate = 1,
        
        [EnumMember]
        FullAmount = 2
    }
}
