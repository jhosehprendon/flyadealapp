using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum PaymentRefundType
    {
        [EnumMember]
        NotAllowed = 0,

        [EnumMember]
        LineItemLevel = 1,

        [EnumMember]
        AccountLevel = 2,

        [EnumMember]
        BookingLevel = 3,

        [EnumMember]
        Unmapped = -1,
    }
}
