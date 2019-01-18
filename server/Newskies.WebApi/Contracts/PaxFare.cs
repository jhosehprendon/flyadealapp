using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxFare
    {
        [DataMember, ValidationInterceptor]
        public string PaxType { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxDiscountCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string FareDiscountCode { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingServiceCharge[] ServiceCharges { get; set; }

        [DataMember, ValidationInterceptor]
        public string TicketFareBasisCode { get; set; }
    }
}