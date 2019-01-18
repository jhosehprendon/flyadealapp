using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Name
    {
        [DataMember, ValidationInterceptor]
        public string Title { get; set; }

        [DataMember, ValidationInterceptor]
        public string FirstName { get; set; }

        [DataMember, ValidationInterceptor]
        public string MiddleName { get; set; }

        [DataMember, ValidationInterceptor]
        public string LastName { get; set; }
    }
}
