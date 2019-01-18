using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class JourneyBaggageResponse
    {
        [DataMember, ValidationInterceptor]
        public int JourneyNumber { get; set; }

        //public Navitaire.WebServices.DataContracts.Common.Enumerations.WeightType WeightType { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerBaggageResponse[] PassengerBaggageResponseList { get; set; }

        //public Navitaire.WebServices.DataContracts.Common.OtherServiceInformation[] OtherServiceInformationList { get; set; }
    }
}
