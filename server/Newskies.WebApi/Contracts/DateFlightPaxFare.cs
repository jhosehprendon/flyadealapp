using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateFlightPaxFare
    {
        [DataMember, ValidationInterceptor]
        public decimal FareAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TaxesAndFeesAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal FarePointAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxType { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxDiscountCode { get; set; }
    }
}
