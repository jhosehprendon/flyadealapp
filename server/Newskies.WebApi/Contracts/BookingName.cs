using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingName
    {
        [DataMember, StringLength(32), ValidationInterceptor]
        //[RegularExpression("^[a-zA-Z]*", ErrorMessage = "First name should not contain spaces, special characters or numbers.")]
        public string FirstName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string MiddleName { get; set; }

        [DataMember, StringLength(32), ValidationInterceptor]
        //[RegularExpression("^[a-zA-Z]*", ErrorMessage = "Last name should not contain spaces, special characters or numbers.")]
        public string LastName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string Suffix { get; set; }

        [DataMember, Title, ValidationInterceptor]
        public string Title { get; set; }

    }
}
