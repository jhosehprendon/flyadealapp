using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindAgentRequestData : RequestBase
    {
        //[DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string LocationGroupCode { get; set; }

        //[DataMember, ValidationInterceptor]
        public string DomainCode { get; set; }

        [DataMember, ValidationInterceptor]
        public SearchString AgentName { get; set; }

        [DataMember, ValidationInterceptor]
        public string LastName { get; set; }

        [DataMember, ValidationInterceptor]
        public SearchString FirstName { get; set; }

        [DataMember, ValidationInterceptor]
        public AgentStatus Status { get; set; }

        [DataMember, ValidationInterceptor]
        public string RoleCode { get; set; }
    }
}
