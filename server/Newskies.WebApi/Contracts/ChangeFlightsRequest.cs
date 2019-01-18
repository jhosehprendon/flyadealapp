using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ChangeFlightsRequest
    {
        [DataMember, /*SellKeyArrayLength(1, 2),*/ ValidationInterceptor]
        public SellKeyList[] JourneySellKeys { get; set; }

        public bool WaiveSeatFee { get; set; }

        public bool ResellSeatSSRs { get; set; }
    }
}
