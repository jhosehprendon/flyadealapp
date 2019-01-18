using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LogonResponse
    {
        public string Signature { get; set; }

        [DataMember]
        public bool MustChangePassword { get; set; }
    }
}