using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PriceItinararyResponse {
        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }
        [DataMember, ValidationInterceptor]
        public Journey[] Journeys { get; set; }
        [DataMember, ValidationInterceptor]
        public BookingSum BookingSum { get; set; }
        [DataMember, ValidationInterceptor]
        public Passenger[] Passengers { get; set; }
    }
}
