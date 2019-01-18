using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetTitleListResponse
    {
        [DataMember, ValidationInterceptor]
        public Title[] TitleList { get; set; }
    }
}
