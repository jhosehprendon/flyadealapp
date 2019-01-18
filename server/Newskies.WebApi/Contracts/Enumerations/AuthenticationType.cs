using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum AuthenticationType
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Password = 1,

        [EnumMember]
        Unmapped = -1,
    }
}
