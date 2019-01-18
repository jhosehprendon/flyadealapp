using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts.Enumerations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ProcessBaggageRequestData
    {
        [DataMember, ValidationInterceptor]
        public ProcessBaggageActionType BaggageActionType { get; set; }

        [DataMember, ValidationInterceptor]
        public string CollectedCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerBaggageRequest[] PassengerBaggageRequestList { get; set; }
    }
}
