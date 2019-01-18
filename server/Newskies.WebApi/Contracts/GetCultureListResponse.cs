using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetCultureListResponse
    {
        [DataMember, ValidationInterceptor]
        public Culture[] CultureList { get; set; }
    }
}
