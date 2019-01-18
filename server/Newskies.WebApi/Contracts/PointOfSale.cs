using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PointOfSale
    {
        [DataMember, ValidationInterceptor]
        public string AgentCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string DomainCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string LocationCode { get; set; }
    }
}
