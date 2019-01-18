using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class TripAvailabilityRequest
    {
        [DataMember, /*AvailabilityRequests(1, 2, 1)*/ ValidationInterceptor]
        public AvailabilityRequest[] AvailabilityRequests { get; set; }
    }
}
