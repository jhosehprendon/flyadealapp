using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingComment
    {
        [DataMember, ValidationInterceptor]
        public CommentType CommentType { get; set; }

        [DataMember, ValidationInterceptor]
        public string CommentText { get; set; }

        [DataMember, ValidationInterceptor]
        public PointOfSale PointOfSale { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime CreatedDate { get; set; }

        [DataMember, ValidationInterceptor]
        public bool SendToBookingSource { get; set; }
    }
}
