using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum FindBookingBy
    {
        [EnumMember]
        AgencyNumber = 7,
        [EnumMember]
        AgentID = 5,
        [EnumMember]
        AgentName = 6,
        [EnumMember]
        BookingDate = 16,// 0x10,
        [EnumMember]
        Contact = 2,
        [EnumMember]
        ContactCustomerNumber = 12,
        [EnumMember]
        CreditCardNumber = 10,
        [EnumMember]
        Customer = 13,
        [EnumMember]
        CustomerNumber = 11,
        [EnumMember]
        EmailAddress = 8,
        [EnumMember]
        Name = 4,
        [EnumMember]
        OrganizationCode = 18, //0x12,
        [EnumMember]
        OSTag = 14,
        [EnumMember]
        PhoneNumber = 9,
        [EnumMember]
        RecordLocator = 1,
        [EnumMember]
        ReferenceNumber = 17, //0x11,
        [EnumMember]
        ThirdPartyRecordLocator = 3,
        [EnumMember]
        TravelDocument = 15,
        [EnumMember]
        Unknown = 0
    }
}
