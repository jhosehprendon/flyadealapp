using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AssignedSeatInfo
    {
        [DataMember, ValidationInterceptor]
        public AssignedSeatJourney[] JourneyList { get; set; }
    }
}
