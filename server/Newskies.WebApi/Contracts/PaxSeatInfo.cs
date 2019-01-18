using System.Collections.Generic;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxSeatInfo
    {
        [DataMember, ValidationInterceptor]
        public short SeatSet { get; set; }

        [DataMember, ValidationInterceptor]
        public short Deck { get; set; }

        [DataMember, ValidationInterceptor]
        public KeyValuePair<string, string>[] Properties { get; set; }
    }
}
