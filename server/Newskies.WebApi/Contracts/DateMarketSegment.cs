using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateMarketSegment
    {
        [DataMember, ValidationInterceptor]
        public string DepartureCity { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalCity { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime DepartureDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string CarrierCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string DateMarketSegmentType { get; set; }
    }
}
