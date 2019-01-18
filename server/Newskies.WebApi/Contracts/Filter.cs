using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Filter
    {
        [DataMember, ValidationInterceptor]
        public DateTime DepartureDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightOrigin { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightDestination { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string SourceOrganization { get; set; }

        [DataMember, ValidationInterceptor]
        public string OrganizationGroupCode { get; set; }
    }
}
