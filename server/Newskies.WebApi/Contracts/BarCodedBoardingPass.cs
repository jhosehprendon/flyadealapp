using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BarCodedBoardingPass
    {
        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        [DataMember, ValidationInterceptor]
        public Name Name { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CustomerNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Operation.ManifestPassengerDoc[] TravelDocs { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SelecteeString { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramLevelShortName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ProgramNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public decimal BaseFare { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ContactPhone { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string FareBasisCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string FareClass { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.Gender Gender { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime InfantDOB { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Name InfantName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.Gender InfantGender { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PassengerTypeTag { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PassportCountry { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime PassportExpiryDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PassportNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime DOB { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime PaymentDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PaymentDescription { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string StationID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public decimal TotalTax { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string ReceiptNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public decimal DiscountedFare { get; set; }

        //[DataMember, ValidationInterceptor]
        //public decimal TotalCost { get; set; }

        //[DataMember, ValidationInterceptor]
        //public decimal TotalFare { get; set; }

        //[DataMember, ValidationInterceptor]
        //public ServiceCharge[] ServiceCharges { get; set; }

        //[DataMember, ValidationInterceptor]
        // public DateTime CurrentTime { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string AgentID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public BarCode BarCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string IATAID { get; set; }

        [DataMember, ValidationInterceptor]
        public BarCodedBoardingPassSegment[] Segments { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string GuestValueLevelCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string GuestValueLevelName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Name ISOName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Name ISOInfantName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool InfantBoardingPass { get; set; }
    }
}