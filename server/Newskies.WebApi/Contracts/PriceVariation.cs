using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PriceVariation
    {
        [DataMember, ValidationInterceptor]
        public decimal BasePrice { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TaxTotal { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxType { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProgramLevel { get; set; }
    }
}
