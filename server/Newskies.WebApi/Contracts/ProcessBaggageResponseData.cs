using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ProcessBaggageResponseData
    {
        [DataMember, ValidationInterceptor]
        public Booking Booking { get; set; }

        [DataMember, ValidationInterceptor]
        public Navitaire.WebServices.DataContracts.Booking.JourneyBaggageResponse[] JourneyBaggageResponseList { get; set; }

        //public Navitaire.WebServices.DataContracts.Common.OtherServiceInformation[] OtherServiceInformationList { get; set; }
    }
}
