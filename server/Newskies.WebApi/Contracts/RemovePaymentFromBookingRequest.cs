using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class RemovePaymentFromBookingRequest
    {
        [DataMember, RemovePaymentFromBooking, ValidationInterceptor]
        public short PaymentNumber { get; set; }
    }
}
