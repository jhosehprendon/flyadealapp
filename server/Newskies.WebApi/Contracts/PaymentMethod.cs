using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaymentMethod
    {
        [DataMember, ValidationInterceptor]
        public bool Allowed { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool AllowDeposit { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool AllowZeroAmount { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool Commissionable { get; set; }

        // [DataMember, ValidationInterceptor]
        public DCCType DCCType { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool DisallowPartialRefund { get; set; }

        [DataMember, ValidationInterceptor]
        public string FeeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }

        [DataMember, ValidationInterceptor]
        public short MaxInstallments { get; set; }

        [DataMember, ValidationInterceptor]
        public string Name { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaymentMethodCode { get; set; }

        [DataMember, ValidationInterceptor]
        public PaymentMethodType PaymentMethodType { get; set; }

        // [DataMember, ValidationInterceptor]
        public PaymentRefundType PaymentRefundType { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool ProportionalRefund { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool RefundableByAgent { get; set; }

        // [DataMember, ValidationInterceptor]
        public Navitaire.WebServices.DataContracts.Common.Enumerations.RefundCurrencyControl RefundCurrencyControl { get; set; }

        // [DataMember, ValidationInterceptor]
        public short RestrictionHours { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool SystemControlled { get; set; }

        // [DataMember, ValidationInterceptor]
        public bool ValidationRequired { get; set; }

        //[DataMember, ValidationInterceptor]
        public Newskies.UtilitiesManager.PaymentMethodField[] PaymentMethodFields { get; set; }
    }
}
