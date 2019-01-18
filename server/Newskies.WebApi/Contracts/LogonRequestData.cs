using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LogonRequestData
    {
        [DataMember, ValidationInterceptor]
        public string DomainCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string AgentName { get; set; }

        [DataMember, ValidationInterceptor]
        public string Password { get; set; }

        [DataMember, ValidationInterceptor]
        public string RoleCode { get; set; }
    }
}
