using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerBaggageResponse
    {
        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public double AdjustedBagWeight { get; set; }

        //public Navitaire.WebServices.DataContracts.Common.OtherServiceInformation[] OtherServiceInformationList { get; set; }
    }
}
