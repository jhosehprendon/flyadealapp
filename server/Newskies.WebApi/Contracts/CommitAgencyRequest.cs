using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitAgencyRequest
    {
        [DataMember, Required, ValidationInterceptor]
        public Organization Organization { get; set; }

        [DataMember, ValidationInterceptor]
        public CommitAgentRequestData CommitAgentRequestData { get; set; }
    }
}
