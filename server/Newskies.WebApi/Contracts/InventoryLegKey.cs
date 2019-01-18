using System;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class InventoryLegKey : FlightDesignator
    {
        [DataMember, ValidationInterceptor]
        public DateTime DepartureDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }
    }
}
