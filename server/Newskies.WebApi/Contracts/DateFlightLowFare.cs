using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateFlightLowFare
    {
        [DataMember, ValidationInterceptor]
        public decimal FareAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TaxesAndFeesAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STA { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STD { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal FarePointAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public short AvailableCount { get; set; }

        [DataMember, ValidationInterceptor]
        public DateFlightLeg[] DateFlightLegList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Booking.AlternateLowFare[] AlternateLowFareList { get; set; }

        [DataMember, ValidationInterceptor]
        public DateFlightFares[] DateFlightFaresList { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProductClass { get; set; }
    }
}
