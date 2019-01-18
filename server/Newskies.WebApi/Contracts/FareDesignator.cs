using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FareDesignator
    {
        [DataMember, ValidationInterceptor]
        public string FareTypeIndicator { get; set; }

        [DataMember, ValidationInterceptor]
        public string CityCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string TravelCityCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string RuleFareTypeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string BaseFareFareClassCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string DowType { get; set; }

        [DataMember, ValidationInterceptor]
        public string SeasonType { get; set; }

        [DataMember, ValidationInterceptor]
        public short RoutingNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string OneWayRoundTrip { get; set; }

        [DataMember, ValidationInterceptor]
        public bool OpenJawAllowed { get; set; }

        [DataMember, ValidationInterceptor]
        public string TripDirection { get; set; }
    }
}