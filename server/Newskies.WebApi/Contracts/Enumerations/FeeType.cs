using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum FeeType
    {
        [EnumMember]
        All = 0,

        [EnumMember]
        Tax = 1,

        [EnumMember]
        TravelFee = 2,

        [EnumMember]
        ServiceFee = 3,

        [EnumMember]
        PaymentFee = 4,

        [EnumMember]
        PenaltyFee = 5,

        [EnumMember]
        SSRFee = 6,

        [EnumMember]
        NonFlightServiceFee = 7,

        [EnumMember]
        UpgradeFee = 8,

        [EnumMember]
        SeatFee = 9,

        [EnumMember]
        BaseFare = 10,

        [EnumMember]
        SpoilageFee = 11,

        [EnumMember]
        NameChangeFee = 12,

        [EnumMember]
        ConvenienceFee = 13,

        [EnumMember]
        BaggageFee = 14,

        [EnumMember]
        FareSurcharge = 15,

        [EnumMember]
        PromotionDiscount = 16,

        [EnumMember]
        ServiceBundle = 17,

        [EnumMember]
        Unmapped = -1,
    }
}
