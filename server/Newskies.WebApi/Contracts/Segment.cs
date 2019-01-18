using System;
using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Segment
    {
        [DataMember, ValidationInterceptor]
        public string ActionStatusCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string CabinOfService { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string SegmentType { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STA { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STD { get; set; }

        [DataMember, ValidationInterceptor]
        public bool International { get; set; }

        [DataMember, ValidationInterceptor]
        public FlightDesignator FlightDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public Fare[] Fares { get; set; }

        [DataMember, ValidationInterceptor]
        public Leg[] Legs { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxSeat[] PaxSeats { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxSSR[] PaxSSRs { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxSegment[] PaxSegments { get; set; }

        [DataMember, ValidationInterceptor]
        public AvailableFare2[] AvailableFares { get; set; }
    }
}