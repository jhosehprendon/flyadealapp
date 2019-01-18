using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxTypeCount
    {
        [DataMember, ValidationInterceptor]
        [StringLength(10, MinimumLength = 3)]
        public string PaxTypeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short PaxCount { get; set; }
    }
}
