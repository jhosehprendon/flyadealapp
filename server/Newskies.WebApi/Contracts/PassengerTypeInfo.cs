using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerTypeInfo
    {
        [DataMember, Required, ValidationInterceptor]
        public DateTime DOB { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public string PaxType { get; set; }
    }
}
