using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindAgentItem
    {
        [DataMember, ValidationInterceptor]
        public long AgentID { get; set; }

        [DataMember, ValidationInterceptor]
        public AgentStatus Status { get; set; }

        [DataMember, ValidationInterceptor]
        public AgentIdentifier AgentIdentifier { get; set; }

        [DataMember, ValidationInterceptor]
        public Name Name { get; set; }

        [DataMember, ValidationInterceptor]
        public bool Allowed { get; set; }
    }
}
