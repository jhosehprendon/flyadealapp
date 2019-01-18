using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ResponseBase
    {

        [DataMember, ValidationInterceptor]
        public short PageSize { get; set; }

        [DataMember, ValidationInterceptor]
        public long LastID { get; set; }

        //public int TotalCount { get; set; }

        //public string CultureCode { get; set; }
    }
}