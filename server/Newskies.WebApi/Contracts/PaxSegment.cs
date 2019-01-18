using System;
using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxSegment
    {
        [DataMember, ValidationInterceptor]
        public string BoardingSequence { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime CreatedDate { get; set; }

        [DataMember, ValidationInterceptor]
        public LiftStatus LiftStatus { get; set; }

        [DataMember, ValidationInterceptor]
        public string OverBookIndicator { get; set; }

        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime PriorityDate { get; set; }

        [DataMember, ValidationInterceptor]
        public TripType TripType { get; set; }

        [DataMember, ValidationInterceptor]
        public bool TimeChanged { get; set; }

        [DataMember, ValidationInterceptor]
        public string VerifiedTravelDocs { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime ModifiedDate { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime ActivityDate { get; set; }

        [DataMember, ValidationInterceptor]
        public short BaggageAllowanceWeight { get; set; }

        [DataMember, ValidationInterceptor]
        public string ServiceBundleCode { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxSegmentCheckInStatus? CheckInStatus { get; set; }
    }
}