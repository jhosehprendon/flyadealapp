using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ApplyPromotionResponse
    {
        [DataMember, ValidationInterceptor]
        public BookingUpdateResponseData BookingUpdateResponseData { get; set; }
    }
}
