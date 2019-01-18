using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ValidationPayment
    {
        [DataMember, ValidationInterceptor]
        public Payment Payment { get; set; }

        [DataMember, ValidationInterceptor]
        public PaymentValidationError[] PaymentValidationErrors { get; set; }
    }
}
