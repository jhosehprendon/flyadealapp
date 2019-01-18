using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SSRPrice
    {
        [DataMember, ValidationInterceptor]
        public string SSRCode { get; set; }

        [DataMember, ValidationInterceptor]
        public PriceVariation[] PriceVariationList { get; set; }

    }
}
