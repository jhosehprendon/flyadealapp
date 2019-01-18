using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindAgentsResponse
    {
        [DataMember, ValidationInterceptor]
        public FindAgentResponseData FindAgentResponseData { get; set; }
    }
}
