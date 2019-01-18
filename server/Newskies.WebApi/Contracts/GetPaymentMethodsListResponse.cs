using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetPaymentMethodsListResponse
    {
        [DataMember, ValidationInterceptor]
        public PaymentMethod[] PaymentMethodList { get; set; }
    }
}
