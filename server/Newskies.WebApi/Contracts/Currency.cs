using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Currency
    {
        [DataMember, ValidationInterceptor]
        public string Name { get; set; }

        [DataMember, ValidationInterceptor]
        public string Code { get; set; }

        [DataMember, ValidationInterceptor]
        public string Symbol { get; set; }
    }
}
