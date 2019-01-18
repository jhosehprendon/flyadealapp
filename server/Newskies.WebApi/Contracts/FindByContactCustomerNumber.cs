using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindByContactCustomerNumber
    {
        [DataMember, ValidationInterceptor]
        public string ContactCustomerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public long AgentID { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public Filter Filter { get; set; }
    }
}
