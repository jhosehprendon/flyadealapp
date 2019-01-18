using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingServiceCharge
    {
        [DataMember, ValidationInterceptor]
        public ChargeType ChargeType { get; set; }

        [DataMember, ValidationInterceptor]
        public CollectType CollectType { get; set; }

        [DataMember, ValidationInterceptor]
        public string ChargeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string TicketCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal Amount { get; set; }

        [DataMember, ValidationInterceptor]
        public string ChargeDetail { get; set; }

        [DataMember, ValidationInterceptor]
        public string ForeignCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal ForeignAmount { get; set; }
    }
}