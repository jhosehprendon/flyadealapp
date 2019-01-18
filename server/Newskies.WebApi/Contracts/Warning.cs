using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Warning
    {
        [DataMember, ValidationInterceptor]
        public string WarningText { get; set; }
    }
}
