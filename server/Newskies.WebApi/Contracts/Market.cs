using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Market
    {
        [DataMember, ValidationInterceptor]
        public string LocationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string TravelLocationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }

        [DataMember, ValidationInterceptor]
        public MarketLocationType LocationType { get; set; }

        [DataMember, ValidationInterceptor]
        public MarketLocationType TravelLocationType { get; set; }

        [DataMember, ValidationInterceptor]
        public Directionality IncludesTaxesAndFees { get; set; }
    }
}
