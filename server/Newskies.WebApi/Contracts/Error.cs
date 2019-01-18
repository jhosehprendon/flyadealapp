using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Error
    {
        [DataMember, ValidationInterceptor]
        public string ErrorText { get; set; }
    }
}
