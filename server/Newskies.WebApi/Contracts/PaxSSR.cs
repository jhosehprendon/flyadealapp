using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxSSR
    {
        [DataMember, ValidationInterceptor]
        public string ActionStatusCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string SSRCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short SSRNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string SSRDetail { get; set; }

        [DataMember, ValidationInterceptor]
        public string FeeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string Note { get; set; }

        [DataMember, ValidationInterceptor]
        public short SSRValue { get; set; }

        [DataMember, ValidationInterceptor]
        public bool IsInServiceBundle { get; set; }
    }
}