using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetDocTypeListResponse
    {
        [DataMember, ValidationInterceptor]
        public DocType[] DocTypeList { get; set; }
    }
}
