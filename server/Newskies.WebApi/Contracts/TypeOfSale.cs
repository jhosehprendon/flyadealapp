using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class TypeOfSale
    {
        [DataMember, ValidationInterceptor]
        public string PaxResidentCountry { get; set; }

        [DataMember, ValidationInterceptor]
        public string PromotionCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] FareTypes { get; set; }
    }
}
