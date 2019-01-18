using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum CommentType
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Itinerary = 1,

        [EnumMember]
        Manifest = 2,

        [EnumMember]
        Alert = 3,

        [EnumMember]
        Archive = 4,

        [EnumMember]
        Unmapped = -1,
    }
}
