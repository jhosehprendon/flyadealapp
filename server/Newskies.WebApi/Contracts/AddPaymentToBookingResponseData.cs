using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AddPaymentToBookingResponseData
    {
        [DataMember, ValidationInterceptor]
        public ValidationPayment ValidationPayment { get; set; }
    }
}
