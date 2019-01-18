using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerTravelDocument
    {
        [DataMember, /*DocType,*/ ValidationInterceptor]
        public string DocTypeCode { get; set; }

        /// [mikev] Treating as "Issuing Country".
        /// Note from NSK Web Service Object Reference help file:
        /// Gets or sets the code of the issuing agency. 
        /// For documents with the DocTypeCode set to FOID, 
        /// the valid IssuedByCodes are: “CC” : CreditCard 
        /// “DL” : DriverLicense “FF” : FrequentFlyer 
        /// "PP" : Passport "NI" : NationalIdentityCard 
        /// "CN" : ConfirmationNumber "TN" : TicketNumber 
        /// "ID" : LocallyDefinedIDNumber 
        [DataMember,/* Country,*/ ValidationInterceptor]
        public string IssuedByCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string DocSuffix { get; set; }

        [DataMember, StringLength(35), ValidationInterceptor]
        public string DocNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime DOB { get; set; }

        [DataMember, ValidationInterceptor]
        public Gender Gender { get; set; }

        //[DataMember, ValidationInterceptor]
        //[CountryValidation]
        //public string Nationality { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime ExpirationDate { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingName[] Names { get; set; }

        //[DataMember, ValidationInterceptor]
        //[CountryValidation]
        //public string BirthCountry { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime IssuedDate { get; set; }

        //[DataMember, ValidationInterceptor]
        public long PersonID { get; set; }

    }
}
