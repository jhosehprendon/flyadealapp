using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetFeeListResponse
    {
        [DataMember, ValidationInterceptor]
        public Fee[] FeeList { get; set; }
    }
}
