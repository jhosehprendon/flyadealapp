using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PersonName
    {
        //[DataMember, ValidationInterceptor]
        public long PersonID { get; set; }

        //[DataMember, ValidationInterceptor]
        public long PersonNameID { get; set; }

        [DataMember, ValidationInterceptor]
        public NameType NameType { get; set; }

        [DataMember, ValidationInterceptor]
        public Name Name { get; set; }
    }
}
