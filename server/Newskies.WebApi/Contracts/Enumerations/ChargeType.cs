using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum ChargeType 
    {
        [EnumMember]
        AddOnServiceCancelFee = 21,//0x15,
        [EnumMember]
        AddOnServiceFee = 17,//0x11,
        [EnumMember]
        AddOnServiceMarkup = 18, //0x12,
        [EnumMember]
        AddOnServicePrice = 9,
        [EnumMember]
        Calculated = 256, //0x100,
        [EnumMember]
        ConnectionAdjustmentAmount = 8,
        [EnumMember]
        Discount = 1,
        [EnumMember]
        DiscountPoints = 11,
        [EnumMember]
        FarePoints = 10,
        [EnumMember]
        FarePrice = 0,
        [EnumMember]
        FareSurcharge = 19, //0x13,
        [EnumMember]
        IncludedAddOnServiceFee = 16, //0x10,
        [EnumMember]
        IncludedTax = 3,
        [EnumMember]
        IncludedTravelFee = 2,
        [EnumMember]
        Loyalty = 20,
        [EnumMember]
        Note = 512, // 0x200,
        [EnumMember]
        PromotionDiscount = 7,
        [EnumMember]
        ServiceCharge = 6,
        [EnumMember]
        Tax = 5,
        [EnumMember]
        TravelFee = 4
    }
}
