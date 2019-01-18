using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    public class PaxLiftStatus
    {
        [DataMember, ValidationInterceptor]
        public int PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public LiftStatus LiftStatus { get; set; }
    }
}