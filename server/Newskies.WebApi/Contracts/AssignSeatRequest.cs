using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AssignSeatRequest
    {
        [DataMember, AssignSeat, ValidationInterceptor]
        public AssignSeatData AssignSeatData { get; set; }
    }
}
