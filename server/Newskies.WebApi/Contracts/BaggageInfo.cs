using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BaggageInfo
    {
        //public string OSTag { get; set; }

        [DataMember, ValidationInterceptor]
        public BaggageStatus BaggageStatus { get; set; }

        //public short Weight { get; set; }

        //public string BaggageType { get; set; }

        //public bool ManualBagTag { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToFlightNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToCarrierCode { get; set; }
    }
}
