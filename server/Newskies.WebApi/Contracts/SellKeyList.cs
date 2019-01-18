using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SellKeyList
    {
        [DataMember, ValidationInterceptor]
        // [JourneySellKey]
        public string JourneySellKey { get; set; }

        [DataMember, ValidationInterceptor]
        // [FareSellKey]
        public string FareSellKey { get; set; }
    }
}
