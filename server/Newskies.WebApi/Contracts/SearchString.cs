using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SearchString
    {
        [DataMember, ValidationInterceptor]
        public string Value { get; set; }

        [DataMember, ValidationInterceptor]
        public SearchType SearchType { get; set; }
    }
}
