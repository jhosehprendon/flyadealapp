using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum FareApplicationType
    {
        [EnumMember]
        Route = 0,

        [EnumMember]
        Sector = 1,

        [EnumMember]
        Governing = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
