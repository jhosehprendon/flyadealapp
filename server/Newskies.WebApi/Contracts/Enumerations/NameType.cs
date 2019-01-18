using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum NameType 
    {

        [EnumMember]
        True = 0,

        [EnumMember]
        Alias = 1,

        [EnumMember]
        Variant = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
