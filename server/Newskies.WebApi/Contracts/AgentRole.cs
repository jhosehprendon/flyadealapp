using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AgentRole : Role
    {
        //[DataMember, ValidationInterceptor]
        public long AgentID { get; set; }

        //[DataMember, ValidationInterceptor]
        public DateTime EffectiveDate { get; set; }

        //[DataMember, ValidationInterceptor]
        public DateTime EndEffectiveDate { get; set; }

        //[DataMember, ValidationInterceptor]
        public DOW EffectiveDOW { get; set; }
    }
}
