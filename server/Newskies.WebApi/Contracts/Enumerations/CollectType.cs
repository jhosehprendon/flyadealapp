using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum CollectType
    {
        [EnumMember]
        SellerChargeable = 0,

        [EnumMember]
        ExternalChargeable = 1,

        [EnumMember]
        SellerNonChargeable = 2,

        [EnumMember]
        ExternalNonChargeable = 3,

        [EnumMember]
        ExternalChargeableImmediate = 4,

        [EnumMember]
        Unmapped = -1,
    }
}
