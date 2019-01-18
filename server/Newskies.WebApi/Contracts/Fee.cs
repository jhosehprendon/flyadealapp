using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Fee
    {
        [DataMember, ValidationInterceptor]
        public bool Allowed { get; set; }
        [DataMember, ValidationInterceptor]
        public string FeeCode { get; set; }
        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }
        [DataMember, ValidationInterceptor]
        public string Name { get; set; }
        [DataMember, ValidationInterceptor]
        public string Description { get; set; }
        [DataMember, ValidationInterceptor]
        public string DisplayCode { get; set; }
        [DataMember, ValidationInterceptor]
        public FeeType FeeType { get; set; }

    }
}
