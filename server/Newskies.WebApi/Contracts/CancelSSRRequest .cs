using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CancelSSRRequest
    {
        [DataMember, SSRRequestData]
        public SSRRequestData SSRRequestData { get; set; }
    }
}
