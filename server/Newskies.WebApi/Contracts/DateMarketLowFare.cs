using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateMarketLowFare
    {
        [DataMember, ValidationInterceptor]
        public string DepartureCity { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalCity { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal FareAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TaxesAndFeesAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime DepartureDate { get; set; }

        // Nick Melnik - comment this out as a redundant information on a backend
        //[DataMember, ValidationInterceptor]
        //public DateTime ExpireUTC { get; set; }

        [DataMember, ValidationInterceptor]
        public bool IncludesTaxesAndFees { get; set; }

        [DataMember, ValidationInterceptor]
        public string CarrierCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string StatusCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal FarePointAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public short AvailableCount { get; set; }

        // Nick Melnik - comment this out as a redundant information on a backend
        //[DataMember, ValidationInterceptor]
        //public DateFlightLowFare[] DateFlightLowFareList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateMarketSegment[] DateMarketSegmentList { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Booking.AlternateLowFare[] AlternateLowFareList { get; set; }
    }
}
