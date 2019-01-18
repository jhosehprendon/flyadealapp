using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitAgentResponseData : AgentResponseData
    {
    }

    [DataContract]
    public class AgentResponseData
    {
        [DataMember, ValidationInterceptor]
        public Person Person { get; set; }

        [DataMember, ValidationInterceptor]
        public Agent Agent { get; set; }
    }
}