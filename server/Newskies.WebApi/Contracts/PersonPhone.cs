using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PersonPhone
    {

        //[DataMember, ValidationInterceptor]
        //public long PersonID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long PersonPhoneID { get; set; }

        [DataMember, ValidationInterceptor]
        public string TypeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string Number { get; set; }

        [DataMember, ValidationInterceptor]
        public string PhoneCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool Default { get; set; }
    }
}
