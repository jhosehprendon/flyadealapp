using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts.Enumerations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BoardingPassRequest
    {
        //[DataMember, ValidationInterceptor]
        public BarCodeType BarCodeType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string RecordLocator { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Name Name { get; set; }

        //[DataMember, ValidationInterceptor]
        //public InventoryLegKey InventoryLegKey { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool BySegment { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool PrintSameDayReturn { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime CurrentTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool Initial { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime InventoryLegKeyDepartureDateTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string BoardingPassSource { get; set; }

        [DataMember, ValidationInterceptor]
        public int JourneyIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int SegmentIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int LegIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public short PaxNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public int BarcodeWidth { get; set; }

        [DataMember, ValidationInterceptor]
        public int BarcodeHeight { get; set; }
    }
}
