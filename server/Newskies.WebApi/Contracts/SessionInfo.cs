using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SessionInfo
    {
        [DataMember, ValidationInterceptor]
        public string AgentName { get; set; }

        [DataMember, ValidationInterceptor]
        public string RoleCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }
    }
}
