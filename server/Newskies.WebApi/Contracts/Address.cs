using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Address
    {
        [DataMember, ValidationInterceptor]
        public string AddressLine1 { get; set; }

        [DataMember, ValidationInterceptor]
        public string AddressLine2 { get; set; }

        [DataMember, ValidationInterceptor]
        public string City { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProvinceState { get; set; }

        [DataMember, ValidationInterceptor]
        public string PostalCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CountryCode { get; set; }
    }
}
