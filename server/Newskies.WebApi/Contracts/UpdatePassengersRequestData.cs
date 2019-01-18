using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class UpdatePassengersRequestData
    {
        [DataMember, /*PassengerArrayLength(1, 9), */ValidationInterceptor]
        public Passenger[] Passengers { get; set; }

        //public bool WaiveNameChangeFee { get; set; }
    }
}
