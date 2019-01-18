using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum PaymentReferenceType
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Booking = 1,

        [EnumMember]
        Session = 2,

        [EnumMember]
        Unmapped = -1,
    }
}
