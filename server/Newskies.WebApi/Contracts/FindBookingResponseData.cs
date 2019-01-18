using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindBookingResponseData
    {
        [DataMember, ValidationInterceptor]
        public int Records { get; set; }

        [DataMember, ValidationInterceptor]
        public long EndingID { get; set; }

        [DataMember, ValidationInterceptor]
        public FindBookingData[] FindBookingDataList { get; set; }
    }
}
