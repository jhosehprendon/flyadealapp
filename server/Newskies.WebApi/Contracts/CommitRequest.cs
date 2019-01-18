using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitRequest
    {
        [DataMember, ValidationInterceptor]
        public bool Flag { get; set; }
    }
}
