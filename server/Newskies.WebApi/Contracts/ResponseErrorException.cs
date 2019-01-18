using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ResponseErrorException : Exception
    {
        public ResponseErrorException(ResponseErrorCode errorCode, string[] messages)
        {
            ErrorCode = errorCode;
            Messages = messages;
        }

        public ResponseErrorException(ResponseErrorCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Messages = new[] { message };
        }

        [DataMember, ValidationInterceptor]
        public ResponseErrorCode ErrorCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] Messages { get; set; }
    }
}
