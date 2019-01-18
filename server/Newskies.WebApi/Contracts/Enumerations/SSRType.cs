using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum SSRType
    {
        [EnumMember]
        Standard = 0,

        [EnumMember]
        Infant = 1,

        [EnumMember]
        Meal = 2,

        [EnumMember]
        BaggageAllowance = 3,

        [EnumMember]
        TravelLineMeal = 4,

        [EnumMember]
        Unmapped = -1,
    }
}
