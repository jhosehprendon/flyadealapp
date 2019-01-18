using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class EquipmentPropertyTypeCodesLookup
    {
        [DataMember, ValidationInterceptor]
        public EquipmentProperty[] BooleanPropertyTypes { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] NumericPropertyTypeCodes { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] SSRCodes { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime Timestamp { get; set; }
    }
}
