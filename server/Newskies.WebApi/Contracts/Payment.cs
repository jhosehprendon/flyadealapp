using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Payment
    {
        // [DataMember, ValidationInterceptor]
        public PaymentReferenceType ReferenceType { get; set; }

        // [DataMember, ValidationInterceptor]
        public long ReferenceID { get; set; }

        [DataMember, ValidationInterceptor]
        public PaymentMethodType PaymentMethodType { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaymentMethodCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal PaymentAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public string CollectedCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal CollectedAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public string QuotedCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal QuotedAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingPaymentStatus Status { get; set; }

        [DataMember, ValidationInterceptor]
        public string AccountNumber { get; set; }

        // [DataMember, ValidationInterceptor]
        public long AccountNumberID { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime Expiration { get; set; }

        // [DataMember, ValidationInterceptor]
        public string AuthorizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public AuthorizationStatus AuthorizationStatus { get; set; }

        // [DataMember, ValidationInterceptor]
        public long ParentPaymentID { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool Transferred { get; set; }

        // [DataMember, ValidationInterceptor]
        public long ReconcilliationID { get; set; }

        // [DataMember, ValidationInterceptor]
        public DateTime FundedDate { get; set; }

        // [DataMember, ValidationInterceptor]
        public short Installments { get; set; }

        // [DataMember, ValidationInterceptor]
        public string PaymentText { get; set; }

        // [DataMember, ValidationInterceptor]
        public ChannelType ChannelType { get; set; }

        [DataMember, ValidationInterceptor]
        public short PaymentNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string AccountName { get; set; }

        // [DataMember, ValidationInterceptor]
        public PointOfSale SourcePointOfSale { get; set; }

        // [DataMember, ValidationInterceptor]
        public PointOfSale PointOfSale { get; set; }

        [DataMember, ValidationInterceptor]
        public long PaymentID { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool Deposit { get; set; }

        // [DataMember, ValidationInterceptor]
        public long AccountID { get; set; }

        // [DataMember, ValidationInterceptor]
        public string Password { get; set; }

        // [DataMember, ValidationInterceptor]
        public string AccountTransactionCode { get; set; }

        // [DataMember, ValidationInterceptor]
        public long VoucherID { get; set; }

        // [DataMember, ValidationInterceptor]
        public long VoucherTransactionID { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool OverrideVoucherRestrictions { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool OverrideAmount { get; set; }

        // [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool PaymentAddedToState { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Booking.DCC DCC { get; set; }

        //[DataMember, ValidationInterceptor]
        //public ThreeDSecure ThreeDSecure { get; set; }

        [DataMember, ValidationInterceptor]
        public PaymentField[] PaymentFields { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Booking.PaymentAddress[] PaymentAddresses { get; set; }

        // [DataMember, ValidationInterceptor]
        public DateTime CreatedDate { get; set; }

        // [DataMember, ValidationInterceptor]
        public long CreatedAgentID { get; set; }

        // [DataMember, ValidationInterceptor]
        public DateTime ModifiedDate { get; set; }

        // [DataMember, ValidationInterceptor]
        public long ModifiedAgentID { get; set; }

        // [DataMember, ValidationInterceptor]
        public int BinRange { get; set; }

        // [DataMember, ValidationInterceptor]
        public DateTime ApprovalDate { get; set; }

        // [DataMember, ValidationInterceptor]
        public long BookingID { get; set; }
    }
}
