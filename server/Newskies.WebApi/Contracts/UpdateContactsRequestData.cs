using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class UpdateContactsRequestData
    {
        [DataMember, /*ArrayLength(1, 1), */ValidationInterceptor]
        public BookingContact[] BookingContactList { get; set; }
    }
}
