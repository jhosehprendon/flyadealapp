using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum SellFeeType
    {
        [EnumMember]
        ServiceFee = 0,

        [EnumMember]
        PenaltyFee = 1,

        [EnumMember]
        Unmapped = -1,
    }
}
