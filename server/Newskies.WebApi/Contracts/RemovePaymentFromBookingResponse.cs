using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class RemovePaymentFromBookingResponse
    {
        [DataMember, ValidationInterceptor]
        public BookingUpdateResponseData BookingUpdateResponseData { get; set; }
    }
}
