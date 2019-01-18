using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SellJourneyByKeyRequestData
    {
        [DataMember, /*SellKeyArrayLength(1, 2),*/ ValidationInterceptor]
        public SellKeyList[] JourneySellKeys { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        // [PaxCountInterceptor(9, 8, 9)]
        public PaxTypeCount[] PaxTypeCounts { get; set; }

        [DataMember, ValidationInterceptor]
        public TypeOfSale TypeOfSale { get; set; }
    }
}
