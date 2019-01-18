using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AssignSeatsResponse
    {
        [DataMember, ValidationInterceptor]
        public BookingUpdateResponseData BookingUpdateResponseData { get; set; }

        [DataMember, ValidationInterceptor]
        public AssignedSeatInfo AssignedSeatInfo { get; set; }
    }
}
