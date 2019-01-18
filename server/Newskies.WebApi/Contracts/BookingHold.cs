using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingHold
    {
        [DataMember, ValidationInterceptor]
        public DateTime HoldDateTime { get; set; }
    }
}
