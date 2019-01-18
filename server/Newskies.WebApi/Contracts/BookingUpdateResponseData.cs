using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingUpdateResponseData
    {
        [DataMember, ValidationInterceptor]
        public Success Success { get; set; }

        [DataMember, ValidationInterceptor]
        public Warning Warning { get; set; }

        [DataMember, ValidationInterceptor]
        public Error Error { get; set; }

        public OtherServiceInformation[] OtherServiceInformations { get; set; }
    }
}
