using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SeatAvailabilityLeg
    {
        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STA { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STD { get; set; }

        [DataMember, ValidationInterceptor]
        public FlightDesignator FlightDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public string AircraftType { get; set; }

        [DataMember, ValidationInterceptor]
        public string AircraftTypeSuffix { get; set; }

        [DataMember, ValidationInterceptor]
        public int EquipmentIndex { get; set; }
    }
}
