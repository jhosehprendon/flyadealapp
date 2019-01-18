using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetPaxTypeListResponse
    {
        [DataMember, ValidationInterceptor]
        public PaxType[] PaxTypeList { get; set; }
    }
}