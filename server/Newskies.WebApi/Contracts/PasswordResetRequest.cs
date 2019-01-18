using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PasswordResetRequest
    {
        [DataMember, Required, ValidationInterceptor]
        public string LoginName { get; set; }

        public string NewPassword { get; set; }
    }
}
