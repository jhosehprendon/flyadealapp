using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BaseAvailabilityRequest
    {
        [DataMember, ValidationInterceptor]
        [StringLength(3, MinimumLength = 3)]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        [StringLength(3, MinimumLength = 3)]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime BeginDate { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime EndDate { get; set; }
    }
}