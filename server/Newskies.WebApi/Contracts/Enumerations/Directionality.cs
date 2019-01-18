using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum Directionality
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        To = 1,

        [EnumMember]
        From = 2,

        [EnumMember]
        Between = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
