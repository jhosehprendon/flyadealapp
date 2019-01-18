using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInPassengersResponseData
    {
        [DataMember, ValidationInterceptor]
        public CheckInMultiplePassengerResponse[] CheckInMultiplePassengerResponseList { get; set; }
    }
}