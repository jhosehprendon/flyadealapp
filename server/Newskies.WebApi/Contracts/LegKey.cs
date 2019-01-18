using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LegKey
    {
        [DataMember, ValidationInterceptor]
        public DateTime DepartureDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }
    }
}
