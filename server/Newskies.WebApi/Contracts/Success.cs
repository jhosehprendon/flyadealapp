using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Success
    {
        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingSum PNRAmount { get; set; }
    }
}
