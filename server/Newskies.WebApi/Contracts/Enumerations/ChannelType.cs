using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum ChannelType
    {
        [EnumMember]
        API = 4,

        [EnumMember]
        Default = 0,

        [EnumMember]
        Direct = 1,

        [EnumMember]
        GDS = 3,

        [EnumMember]
        Web = 2
    }

}
