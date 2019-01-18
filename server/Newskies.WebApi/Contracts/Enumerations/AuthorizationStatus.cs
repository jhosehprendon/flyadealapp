using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum AuthorizationStatus
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        Acknowledged = 1,

        [EnumMember]
        Pending = 2,

        [EnumMember]
        InProcess = 3,

        [EnumMember]
        Approved = 4,

        [EnumMember]
        Declined = 5,

        [EnumMember]
        Referral = 6,

        [EnumMember]
        PickUpCard = 7,

        [EnumMember]
        HotCard = 8,

        [EnumMember]
        Voided = 9,

        [EnumMember]
        Retrieval = 10,

        [EnumMember]
        ChargedBack = 11,

        [EnumMember]
        Error = 12,

        [EnumMember]
        ValidationFailed = 13,

        [EnumMember]
        Address = 14,

        [EnumMember]
        VerificationCode = 15,

        [EnumMember]
        FraudPrevention = 16,

        [EnumMember]
        Unmapped = -1,
    }
}
