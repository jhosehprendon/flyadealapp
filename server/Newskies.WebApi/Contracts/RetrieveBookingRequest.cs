using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class RetrieveBookingRequest
    {
        [DataMember, StringLength(6, MinimumLength = 6), RegularExpression("^[a-zA-Z0-9]+$"), ValidationInterceptor]
        public string RecordLocator { get; set; }
    }
}
