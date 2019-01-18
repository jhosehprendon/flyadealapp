using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInPassengersResponse
    {
        [DataMember, ValidationInterceptor]
        public CheckInPassengersResponseData CheckInPassengersResponseData { get; set; }
    }
}
