using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum WeightCategory
    {

        [EnumMember]
        Male = 0,

        [EnumMember]
        Female = 1,

        [EnumMember]
        Child = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
