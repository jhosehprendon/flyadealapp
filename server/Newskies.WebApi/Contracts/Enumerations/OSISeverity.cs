using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    public enum OSISeverity
    {
        [EnumMember]
        General = 0,

        [EnumMember]
        Warning = 1,

        [EnumMember]
        Critical = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
