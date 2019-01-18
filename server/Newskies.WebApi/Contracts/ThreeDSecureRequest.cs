using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ThreeDSecureRequest
    {
        [DataMember, ValidationInterceptor]
        public string BrowserUserAgent { get; set; }

        [DataMember, ValidationInterceptor]
        public string BrowserAccept { get; set; }

        [DataMember, ValidationInterceptor]
        public string RemoteIpAddress { get; set; }

        [DataMember, ValidationInterceptor]
        public string TermUrl { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProxyVia { get; set; }
    }
}
