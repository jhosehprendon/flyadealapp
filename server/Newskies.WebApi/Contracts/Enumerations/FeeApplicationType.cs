using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    public enum FeeApplicationType
    {
        [EnumMember]
        Journey = 2,

        [EnumMember]
        Leg = 4,

        [EnumMember]
        None = 0,

        [EnumMember]
        PNR = 1,

        [EnumMember]
        Segment = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
