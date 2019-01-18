using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AgentIdentifier
    {
        [DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string DomainCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string AgentName { get; set; }
    }
}
