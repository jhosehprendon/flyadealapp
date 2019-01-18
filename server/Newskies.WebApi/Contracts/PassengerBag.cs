using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerBag
    {
        [DataMember, ValidationInterceptor]
        public long BaggageID { get; set; }

        [DataMember, ValidationInterceptor]
        public string OSTag { get; set; }

        [DataMember, ValidationInterceptor]
        public System.DateTime OSTagDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string StationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short Weight { get; set; }

        [DataMember, ValidationInterceptor]
        public WeightType WeightType { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToFlightNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public bool LRTIndicator { get; set; }

        [DataMember, ValidationInterceptor]
        public string BaggageType { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToCarrierCode { get; set; }
    }
}
