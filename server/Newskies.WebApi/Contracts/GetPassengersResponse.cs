using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetPassengersResponse
    {
        [DataMember, ValidationInterceptor]
        public Passenger[] Passengers { get; set; }
    }
}
