using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitAgencyResponse
    {
        [DataMember, ValidationInterceptor]
        public Organization Organization { get; set; }

        [DataMember, ValidationInterceptor]
        public CommitAgentResponseData CommitAgentResponseData { get; set; }
    }
}
