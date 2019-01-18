using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class TripAvailabilityResponse
    {
        [DataMember, ValidationInterceptor]
        public JourneyDateMarket[][] Schedules { get; set; }

        [DataMember, ValidationInterceptor]
        public Fare[] Fares { get; set; }

    }
}
