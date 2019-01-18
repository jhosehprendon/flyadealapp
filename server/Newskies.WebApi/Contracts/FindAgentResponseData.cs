using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindAgentResponseData : ResponseBase
    {
        [DataMember, ValidationInterceptor]
        public FindAgentItem[] FindAgentList { get; set; }
    }
}
