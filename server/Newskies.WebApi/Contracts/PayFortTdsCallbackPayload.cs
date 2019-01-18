using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PayFortTdsCallbackPayload {
        [DataMember, ValidationInterceptor]
        public string response_message { get; set; }
    }
}
