using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Title
    {
        // [DataMember, ValidationInterceptor]
        public bool Allowed { get; set; }

        [DataMember, ValidationInterceptor]
        public string TitleKey { get; set; }

        [DataMember, ValidationInterceptor]
        public string Description { get; set; }

        [DataMember, ValidationInterceptor]
        public Gender Gender { get; set; }

        [DataMember, ValidationInterceptor]
        public WeightCategory WeightCategory { get; set; }
    }
}
