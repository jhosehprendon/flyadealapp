using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Person
    {
        //[DataMember, ValidationInterceptor]
        public long PersonID { get; set; }

        //[DataMember, ValidationInterceptor]
        public PersonType PersonType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PersonStatus PersonStatus { get; set; }

        [DataMember, ValidationInterceptor]
        public string CultureCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime DOB { get; set; }

        [DataMember, ValidationInterceptor]
        public Gender Gender { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string NationalIDNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string TypeBAddress { get; set; }

        //[DataMember, ValidationInterceptor]
        public string CustomerNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short TrustLevel { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PaxType { get; set; }

        [DataMember, ValidationInterceptor]
        public string Nationality { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ResidentCountry { get; set; }

        //[DataMember, ValidationInterceptor]
        //public NotificationPreference NotificationPreference { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonAddress[] PersonAddressList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonComment[] PersonComments { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonEMail[] PersonEMailList { get; set; }
        [DataMember, EmailAddress, ValidationInterceptor]
        public string EmailAddress { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonFOP[] PersonFOPList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonInfo[] PersonInfoList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PersonName[] PersonNameList { get; set; }
        [DataMember, ValidationInterceptor]
        public Name Name { get; set; }
        
        //[DataMember, ValidationInterceptor]
        //public PersonPhone[] PersonPhoneList { get; set; }
        [DataMember, ValidationInterceptor]
        public string MobilePhone { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerTravelDocument[] TravelDocs { get; set; }

        [DataMember, ValidationInterceptor]
        public string ResidentCountry { get; set; }

        [DataMember, ValidationInterceptor]
        public string City { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonCustomerProgram[] PersonCustomerPrograms { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonAttachment[] PersonAttachments { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonContact[] PersonContactList { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonUnitPreference[] PersonUnitPreferences { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Person.PersonAffiliation[] PersonAffiliationList { get; set; }
    }
}
