using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CommitResponse
    {
        [DataMember, ValidationInterceptor]
        public BookingUpdateResponseData BookingUpdateResponseData { get; set; }
    }
}
