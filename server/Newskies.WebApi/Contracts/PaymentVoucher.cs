using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaymentVoucher
    {
        [DataMember, ValidationInterceptor]
        public long VoucherIDField { get; set; }

        [DataMember, ValidationInterceptor]
        public long VoucherTransaction { get; set; }

        [DataMember, ValidationInterceptor]
        public bool OverrideVoucherRestrictions { get; set; }

        [DataMember, ValidationInterceptor]
        public bool OverrideAmount { get; set; }

        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }
    }
}
