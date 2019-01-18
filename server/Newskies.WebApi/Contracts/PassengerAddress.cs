using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerAddress
    {
        [DataMember, ValidationInterceptor]
        public string TypeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string StationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CompanyName { get; set; }

        [DataMember, ValidationInterceptor]
        public string AddressLine1 { get; set; }

        [DataMember, ValidationInterceptor]
        public string AddressLine2 { get; set; }

        [DataMember, ValidationInterceptor]
        public string AddressLine3 { get; set; }

        [DataMember, ValidationInterceptor]
        public string City { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProvinceState { get; set; }

        [DataMember, ValidationInterceptor]
        public string PostalCode { get; set; }

        [DataMember, /*Country,*/ ValidationInterceptor]
        public string CountryCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string Phone { get; set; }

        [DataMember, ValidationInterceptor]
        public string EmailAddress { get; set; }

        [DataMember, ValidationInterceptor]
        public string CultureCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool RefusedContact { get; set; }
    }
}
