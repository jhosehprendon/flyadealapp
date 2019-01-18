using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingPointOfSale
    {
        [DataMember, ValidationInterceptor]
        public string AgentCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationCode { get; set; }
    }
}
