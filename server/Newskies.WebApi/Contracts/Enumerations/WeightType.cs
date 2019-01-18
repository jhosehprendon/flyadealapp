using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum WeightType
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Pounds = 1,

        [EnumMember]
        Kilograms = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
