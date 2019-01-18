using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AvailabilityRequest : BaseAvailabilityRequest
    {
        [DataMember, /*PaxCountInterceptor(9, 8, 9), */ValidationInterceptor]
        public PaxTypeCount[] PaxTypeCounts { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, /*Country,*/ ValidationInterceptor]
        public string PaxResidentCountry { get; set; }

        [DataMember, StringLength(32), ValidationInterceptor]
        public string PromotionCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] FareTypes { get; set; }
    }
}
