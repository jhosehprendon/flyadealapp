using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PasswordSetRequest
    {
        [DataMember, Required, ValidationInterceptor]
        public string NewPassword { get; set; }
    }
}
