using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    public enum RequestPaymentMethodType
    {
        [EnumMember]
        PrePaid = 0,

        [EnumMember]
        ExternalAccount = 1,

        [EnumMember]
        AgencyAccount = 2,

        [EnumMember]
        CreditShell = 3,

        [EnumMember]
        CreditFile = 4,

        [EnumMember]
        Voucher = 5,

        [EnumMember]
        Loyalty = 6,

        [EnumMember]
        Unmapped = -1,
    }
}
