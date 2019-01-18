using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Station
    {
        [DataMember, ValidationInterceptor]
        public bool Allowed { get; set; }

        [DataMember, ValidationInterceptor]
        public string CityCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CountryCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CultureCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }

        [DataMember, ValidationInterceptor]
        public string Latitude { get; set; }

        [DataMember, ValidationInterceptor]
        public string Longitude { get; set; }

        [DataMember, ValidationInterceptor]
        public string MACCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string Name { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProvinceStateCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string ShortName { get; set; }

        [DataMember, ValidationInterceptor]
        public string StationClass { get; set; }

        [DataMember, ValidationInterceptor]
        public string StationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string TimeZoneCode { get; set; }
    }
}