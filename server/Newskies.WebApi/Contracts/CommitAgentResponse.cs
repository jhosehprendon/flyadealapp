using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitAgentResponse
    {
        [DataMember, ValidationInterceptor]
        public CommitAgentResponseData CommitAgentResData { get; set; }
    }
}
