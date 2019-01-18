using Newskies.WebApi.Validation;
using System.Runtime.Serialization; 

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SellSSRRequest
    {
        [DataMember, SSRRequestData, /* ValidationInterceptor(ValidationInterceptorCustomFlag.FLAG0)*/ ValidationInterceptor]
        public SSRRequestData SSRRequestData { get; set; }
    }
}
