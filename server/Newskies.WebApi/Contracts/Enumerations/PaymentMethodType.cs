using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum PaymentMethodType
    {
        [EnumMember]
        PrePaid = 0,

        [EnumMember]
        ExternalAccount = 1,

        [EnumMember]
        AgencyAccount = 2,

        [EnumMember]
        CustomerAccount = 3,

        [EnumMember]
        Voucher = 4,

        [EnumMember]
        Loyalty = 5,

        [EnumMember]
        Unmapped = -1,
    }
}
