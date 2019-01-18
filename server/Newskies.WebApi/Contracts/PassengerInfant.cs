using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerInfant
    {
        [DataMember, ValidationInterceptor]
        public DateTime DOB { get; set; }

        [DataMember, ValidationInterceptor]
        public Gender Gender { get; set; }

        [DataMember, ValidationInterceptor]
        // [Country]
        public string Nationality { get; set; }

        [DataMember, ValidationInterceptor]
        // [Country]
        public string ResidentCountry { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingName[] Names { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PaxType { get; set; }
    }
}
