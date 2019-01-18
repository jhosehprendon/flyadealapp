using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindByRecordLocator
    {
        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        [DataMember, ValidationInterceptor]
        public string SourceOrganization { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationGroupCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public long AgentID { get; set; }

    }
}
