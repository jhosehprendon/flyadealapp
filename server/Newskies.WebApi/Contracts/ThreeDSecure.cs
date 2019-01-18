using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    public class ThreeDSecure
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

        [DataMember, ValidationInterceptor]
        public bool ValidationTDSApplicable { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSPaReq { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSAcsUrl { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSPaRes { get; set; }

        [DataMember, ValidationInterceptor]
        public bool ValidationTDSSuccessful { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSAuthResult { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSCavv { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSCavvAlgorithm { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSEci { get; set; }

        [DataMember, ValidationInterceptor]
        public string ValidationTDSXid { get; set; }
    }
}
