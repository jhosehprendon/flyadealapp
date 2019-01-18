using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Country
    {
        [DataMember, ValidationInterceptor]
        public string CountryCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool HasProvinceStates { get; set; }

        [DataMember, ValidationInterceptor]
        public string Name { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }

        [DataMember, ValidationInterceptor]
        public string CountryCode3C { get; set; }

        [DataMember, ValidationInterceptor]
        public string DefaultCurrencyCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool APISRequiredOutbound { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool APISRequiredInbound { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool APPSRequiredOutbound { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool APPSRequiredInbound { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool APISCheckDocuments { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool AqqRequired { get; set; }
    }
}
