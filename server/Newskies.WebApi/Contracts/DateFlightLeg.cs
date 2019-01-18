using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateFlightLeg
    {
        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STA { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STD { get; set; }

        [DataMember, ValidationInterceptor]
        public string CarrierCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string OperatingCarrier { get; set; }

        [DataMember, ValidationInterceptor]
        public string EquipmentType { get; set; }
    }
}
