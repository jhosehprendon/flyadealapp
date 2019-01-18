using System.Runtime.Serialization; using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts.Enumerations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AssignSeatData
    {
        public AssignSeatData()
        {
            SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
        }

        [DataMember, ValidationInterceptor]
        public int JourneyIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int SegmentIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public int LegIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public short? PaxNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string CompartmentDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public string UnitDesignator { get; set; }

        public bool WaiveFees { get; set; }

        public SeatAssignmentMode SeatAssignmentMode { get; set; }
    }
}
