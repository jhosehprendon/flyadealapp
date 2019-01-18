using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum LiftStatus
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        CheckedIn = 1,

        [EnumMember]
        Boarded = 2,

        [EnumMember]
        NoShow = 3,

        [EnumMember]
        Unmapped = -1
    }
}
