using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SellResponse
    {
        [DataMember, ValidationInterceptor]
        public BookingUpdateResponseData BookingUpdateResponseData { get; set; }
    }
}
