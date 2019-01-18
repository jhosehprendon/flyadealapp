using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingSum
    {
        [DataMember, ValidationInterceptor]
        public decimal BalanceDue { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal AuthorizedBalanceDue { get; set; }

        [DataMember, ValidationInterceptor]
        public short SegmentCount { get; set; }

        [DataMember, ValidationInterceptor]
        public short PassiveSegmentCount { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TotalCost { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal PointsBalanceDue { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TotalPointCost { get; set; }

        [DataMember, ValidationInterceptor]
        public string AlternateCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal AlternateCurrencyBalanceDue { get; set; }
    }
}
