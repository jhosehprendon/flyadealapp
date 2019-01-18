using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetCurrencyListResponse
    {
        [DataMember, ValidationInterceptor]
        public Currency[] CurrencyList { get; set; }
    }
}