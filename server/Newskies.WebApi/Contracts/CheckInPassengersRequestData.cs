using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInPassengersRequestData
    {
        [DataMember, CheckInMultiplePassengerRequest, ValidationInterceptor]
        public CheckInMultiplePassengerRequest[] CheckInMultiplePassengerRequestList { get; set; }
    }
}
