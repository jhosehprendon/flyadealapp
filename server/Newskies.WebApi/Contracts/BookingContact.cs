using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingContact
    {
        //[DataMember, ValidationInterceptor]
        //public string TypeCode { get; set; }

        [DataMember, /*ArrayLength(1, 1), */ValidationInterceptor]
        public BookingName[] Names { get; set; }

        [DataMember, EmailAddress, ValidationInterceptor]
        public string EmailAddress { get; set; }

        [DataMember, Phone, ValidationInterceptor]
        public string HomePhone { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string WorkPhone { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string OtherPhone { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string Fax { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CompanyName { get; set; }

        [DataMember, ValidationInterceptor]
        public string AddressLine1 { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string AddressLine2 { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string AddressLine3 { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public string City { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProvinceState { get; set; }

        [DataMember, ValidationInterceptor]
        public string PostalCode { get; set; }

        [DataMember, /*Country,*/ ValidationInterceptor]
        public string CountryCode { get; set; }

        [DataMember, /*Culture,*/ ValidationInterceptor]
        public string CultureCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.DistributionOption DistributionOption { get; set; }

        [DataMember, ValidationInterceptor]
        public string CustomerNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.NotificationPreference NotificationPreference { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SourceOrganization { get; set; }
    }
}
