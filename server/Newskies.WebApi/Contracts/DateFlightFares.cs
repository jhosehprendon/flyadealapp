using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DateFlightFares
    {
        [DataMember, ValidationInterceptor]
        public short NightsStay { get; set; }

        [DataMember, ValidationInterceptor]
        public DateFlightFare[] DateFlightFareList { get; set; }
    }
}
