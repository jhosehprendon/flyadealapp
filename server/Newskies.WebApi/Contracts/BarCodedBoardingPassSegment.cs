using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{

    [DataContract]
    public class BarCodedBoardingPassSegment
    {
        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public InventoryLegKey InventoryLegKey { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string BookingStatus { get; set; }

        [DataMember, ValidationInterceptor]
        public BarCodedBoardingPassLeg[] Legs { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ClassOfService { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool International { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string VerifiedTravelDocs { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool InfantInd { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CabinOfService { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string TicketIndicator { get; set; }

        //[DataMember, ValidationInterceptor]
        // public string TicketNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string InfantTicketNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProductClassName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SegmentType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string XRefCarrierCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string XRefFlightNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string XRefOpSuffix { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string FareClassName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public System.DateTime ArrivalTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Operation.BarCode[] BarCodes { get; set; }

        [DataMember, ValidationInterceptor]
        public System.DateTime BoardingTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool ConnectionInd { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureGate { get; set; }

        [DataMember, ValidationInterceptor]
        public System.DateTime DepartureTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string XRefCarrierName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string XRefClassOfService { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string AirlineName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string XRefRecordLocator { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramLevelShortName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramCode { get; set; }

        //[DataMember, ValidationInterceptor]
        // public string FreeBaggageAllowance { get; set; }
    }
}