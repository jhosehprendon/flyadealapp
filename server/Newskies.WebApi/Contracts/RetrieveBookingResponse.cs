using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class RetrieveBookingResponse
    {
        [DataMember, ValidationInterceptor]
        public Booking Booking { get; set; }
    }
}
