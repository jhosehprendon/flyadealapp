using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum BookingPaymentStatus
    {
        [EnumMember]
        New = 0,

        [EnumMember]
        Received = 1,

        [EnumMember]
        Pending = 2,

        [EnumMember]
        Approved = 3,

        [EnumMember]
        Declined = 4,

        [EnumMember]
        Unknown = 5,

        [EnumMember]
        PendingCustomerAction = 6,

        [EnumMember]
        Unmapped = -1,
    }
}
