using System;
using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AddPaymentToBookingRequestData
    {
        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.MessageState MessageState { set; get; }

        //[DataMember, ValidationInterceptor]
        public RequestPaymentMethodType PaymentMethodType { set; get; }

        //[DataMember, ValidationInterceptor]
        public string QuotedCurrencyCode { set; get; }

        //[DataMember, ValidationInterceptor]
        public decimal QuotedAmount { set; get; }

        [DataMember, StringLength(10, MinimumLength = 2), ValidationInterceptor]
        public string PaymentMethodCode { set; get; }

        //[DataMember, ValidationInterceptor]
        //public long AccountNumberID { set; get; }

        [DataMember, ValidationInterceptor]
        public string AccountNumber { set; get; }

        [DataMember, ValidationInterceptor]
        public DateTime? Expiration { set; get; }

        //[DataMember, ValidationInterceptor]
        //public long ParentPaymentID { set; get; }

        //public string PaymentText { set; get; }

        //[DataMember, ValidationInterceptor]
        public PaymentField[] PaymentFields { set; get; }

        //[DataMember, ValidationInterceptor]
        public int Installments { get; set; }

        [DataMember, StringLength(100), ValidationInterceptor]
        public string AccountHolderName { get; set; }

        [DataMember, ValidationInterceptor]
        public string CVVCode { get; set; }

        //public Navitaire.WebServices.DataContracts.Booking.PaymentAddress[] PaymentAddresses { set; get; }

        //public Navitaire.WebServices.DataContracts.Booking.AgencyAccount AgencyAccount { set; get; }

        //public Navitaire.WebServices.DataContracts.Booking.CreditShell CreditShell { set; get; }

        //public Navitaire.WebServices.DataContracts.Booking.CreditFile CreditFile { set; get; }

        //[DataMember, ValidationInterceptor]
        public PaymentVoucher PaymentVoucher { set; get; }

        //[DataMember, ValidationInterceptor]
        public ThreeDSecureRequest ThreeDSecureRequest { set; get; }

        //public Navitaire.WebServices.DataContracts.Booking.MCCRequest MCCRequest { set; get; }

        //[DataMember, ValidationInterceptor]
        //public string AuthorizationCode { set; get; }
    }
}
