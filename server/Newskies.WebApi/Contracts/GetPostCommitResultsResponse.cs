using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetPostCommitResultsResponse
    {
        [DataMember, ValidationInterceptor]
        public int MaxQueryCount { get; set; }

        [DataMember, ValidationInterceptor]
        public int RepeatQueryIntervalSecs { get; set; }

        [DataMember, ValidationInterceptor]
        public bool ShouldContinuePolling { get; set; }

        [DataMember, ValidationInterceptor]
        public int SessionPaymentQueryCount { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime PaymentQueryLastCall { get; set; }

        [DataMember, ValidationInterceptor]
        public string RedirectPaymentURL { get; set; }

        [DataMember, ValidationInterceptor]
        public string RedirectMethod { get; set; }

        [DataMember, ValidationInterceptor]
        public PaymentField[] RedirectParams { get; set; }

        [DataMember, ValidationInterceptor]
        public Booking BookingDelta { get; set; }
    }
}
