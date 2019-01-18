using System.Runtime.Serialization; using Newskies.WebApi.Validation;


namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Journey
    {
        [DataMember, ValidationInterceptor]
        public Segment[] Segments { get; set; }

        [DataMember, ValidationInterceptor]
        public string JourneySellKey { get; set; }
    }
}
