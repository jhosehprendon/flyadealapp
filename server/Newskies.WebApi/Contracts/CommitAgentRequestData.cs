using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitAgentRequestData
    {
        [DataMember, Required, ValidationInterceptor]
        public Person Person { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public Agent Agent { get; set; }
    }
}
