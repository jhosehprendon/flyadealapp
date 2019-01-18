using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetMarketListResponse
    {
        [DataMember, ValidationInterceptor]
        public Market[] MarketList { get; set; }
    }
}