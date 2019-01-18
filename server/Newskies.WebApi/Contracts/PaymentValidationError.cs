using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts.Enumerations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaymentValidationError
    {
        [DataMember, ValidationInterceptor]
        public PaymentValidationErrorType ErrorType { get; set; }

        public string ErrorDescription { get; set; }

        public string AttributeName { get; set; }
    }
}
