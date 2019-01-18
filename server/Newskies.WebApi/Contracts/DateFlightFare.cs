using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateFlightFare
    {
        [DataMember, ValidationInterceptor]
        public decimal FareAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TaxesAndFeesAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal FarePointAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public short AvailableCount { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] BookingClassList { get; set; }

        [DataMember, ValidationInterceptor]
        public bool IsRouteAU { get; set; }

        [DataMember, ValidationInterceptor]
        public short RouteAdjustment { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProductClass { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxType { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxDiscountCode { get; set; }

        [DataMember, ValidationInterceptor]
        public DateFlightPaxFare[] DateFlightPaxFareList { get; set; }
    }
}
