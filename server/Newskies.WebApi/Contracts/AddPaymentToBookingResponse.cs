using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AddPaymentToBookingResponse
    {
        [DataMember, ValidationInterceptor]
        public AddPaymentToBookingResponseData BookingPaymentResponse { get; set; }
    }
}
