using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerInfo
    {
        [DataMember, ValidationInterceptor]
        public decimal BalanceDue { get; set; }

        [DataMember, ValidationInterceptor]
        public Gender Gender { get; set; }

        [DataMember, ValidationInterceptor]
        // [Country]
        public string Nationality { get; set; }

        [DataMember, ValidationInterceptor]
        // [Country]
        public string ResidentCountry { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal TotalCost { get; set; }
    }
}
