using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInMultiplePassengerResponse
    {
        [DataMember, ValidationInterceptor]
        public InventoryLegKey InventoryLegKey { get; set; }

        [DataMember, ValidationInterceptor]
        public string ReturnDepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string ReturnArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public FlightDesignator ReturnFlightDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public Segment[] ContractSegments { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Operation.DownlineSegment[] DownlineSegments { get; set; }

        [DataMember, ValidationInterceptor]
        public Segment[] IATCISegments { get; set; }

        [DataMember, ValidationInterceptor]
        public CheckInPaxResponse[] CheckInPaxResponseList { get; set; }
    }
}