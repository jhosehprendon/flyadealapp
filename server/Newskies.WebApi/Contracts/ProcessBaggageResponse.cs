using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ProcessBaggageResponse
    {
        [DataMember, ValidationInterceptor]
        public ProcessBaggageResponseData ProcessBaggageResponseData { get; set; }
    }
}
