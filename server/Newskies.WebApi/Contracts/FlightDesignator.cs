using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FlightDesignator
    {
        [DataMember, ValidationInterceptor]
        public string CarrierCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string OpSuffix { get; set; }
    }
}