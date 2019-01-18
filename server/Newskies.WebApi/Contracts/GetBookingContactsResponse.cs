using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetBookingContactsResponse
    {
        [DataMember, ValidationInterceptor]
        public BookingContact[] BookingContacts { get; set; }
    }
}
