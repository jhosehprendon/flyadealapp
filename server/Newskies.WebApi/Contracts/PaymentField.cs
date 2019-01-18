using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaymentField
    {
        [DataMember, ValidationInterceptor]
        public string FieldName { get; set; }

        [DataMember, ValidationInterceptor]
        public string FieldValue { get; set; }
    }
}
