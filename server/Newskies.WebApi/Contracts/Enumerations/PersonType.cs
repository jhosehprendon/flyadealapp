using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum PersonType
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Customer = 1,

        [EnumMember]
        Agent = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
