using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum MessageState
    { 
        [EnumMember]
        New = 0,

        [EnumMember]
        Clean = 1,

        [EnumMember]
        Modified = 2,

        [EnumMember]
        Deleted = 3,

        [EnumMember]
        Confirmed = 4,

        [EnumMember]
        Unmapped = -1,
    }
}
