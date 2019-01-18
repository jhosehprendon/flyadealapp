using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInError
    {
        [DataMember, ValidationInterceptor]
        public short TypeId{ get; set; }

        [DataMember, ValidationInterceptor]
        public string ErrorMessage{ get; set; }

        [DataMember, ValidationInterceptor]
        public InventoryLegKey InventoryLegKey{ get; set; }

        [DataMember, ValidationInterceptor]
        public CheckInError InnerError{ get; set; }
    }
}