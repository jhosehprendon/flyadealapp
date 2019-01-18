using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CancelFeeRequestData
    {
        [DataMember, ValidationInterceptor]
        public string FeeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }
    }
}
