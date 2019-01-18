using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PasswordSetAnonymouslyRequest
    {
        [DataMember, ValidationInterceptor]
        public LogonRequestData LogonRequestData { get; set; }

        [DataMember, ValidationInterceptor]
        public PasswordSetRequest PasswordSetRequest { get; set; }
    }
}
