using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum Gender
    {
        [EnumMember]
        Male = 0,

        [EnumMember]
        Female = 1,

        [EnumMember]
        Unmapped = -1,
    }
}
