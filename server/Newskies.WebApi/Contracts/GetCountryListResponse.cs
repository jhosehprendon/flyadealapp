using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetCountryListResponse
    {
        [DataMember, ValidationInterceptor]
        public Country[] CountryList { get; set; }
    }
}
