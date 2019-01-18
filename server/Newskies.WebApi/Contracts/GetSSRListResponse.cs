using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetSSRListResponse
    {
        [DataMember, ValidationInterceptor]
        public SSR[] SSRList { get; set; }
    }
}
