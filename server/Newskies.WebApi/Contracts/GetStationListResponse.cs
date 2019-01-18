using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetStationListResponse
    {
        [DataMember, ValidationInterceptor]
        public Station[] StationList { get; set; }
    }
}