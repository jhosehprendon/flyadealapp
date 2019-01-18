using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LowFareAvailabilityResponse
    {
        [DataMember, ValidationInterceptor]
        public DateMarketLowFare[] DateMarketLowFareList { get; set; }
    }
}
