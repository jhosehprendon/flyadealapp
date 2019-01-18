using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInMultiplePassengerRequest
    {
        //[DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        //[DataMember, ValidationInterceptor]
        public InventoryLegKey InventoryLegKey { get; set; }

        //[DataMember, ValidationInterceptor]
        //public LiftStatus LiftStatus { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool BySegment { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PassengerStatusCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool CheckSameDayReturn { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool SkipSecurityChecks { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool SeatRequired { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool RetrieveBoardingZone { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool AllowPartialCheckIn { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool OtherAirlineCheckin { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CheckInDestination { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool ReturnDownlineSegments { get; set; }

        //[DataMember, ValidationInterceptor]
        public DateTime InventoryLegKeyDepartureDateTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool ProcessDownlineIATCI { get; set; }

        //[DataMember, ValidationInterceptor]
        public CheckInPaxRequest[] CheckInPaxRequestList { get; set; }

        [DataMember, ValidationInterceptor]
        public int JourneyIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int SegmentIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int[] PassengerNumbers { get; set; }
    }
}