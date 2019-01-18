using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ResponseError
    {
        public ResponseErrorCode ErrorCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] Message { get; set; }
    }
}
