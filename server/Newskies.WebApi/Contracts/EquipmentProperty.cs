using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class EquipmentProperty
    {
        [DataMember, ValidationInterceptor]
        public string TypeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string Value { get; set; }
    }
}
