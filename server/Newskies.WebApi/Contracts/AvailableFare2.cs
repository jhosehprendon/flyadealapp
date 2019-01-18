using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AvailableFare2
    {
        [DataMember, ValidationInterceptor]
        public short AvailableCount { get; set; }

        [DataMember, ValidationInterceptor]
        public ClassStatus Status { get; set; }

        [DataMember, ValidationInterceptor]
        public int FareIndex { get; set; }

        [DataMember, ValidationInterceptor]
        public string ServiceBundleSetCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short[] ServiceBundleOfferIndexes { get; set; }
    }
}
