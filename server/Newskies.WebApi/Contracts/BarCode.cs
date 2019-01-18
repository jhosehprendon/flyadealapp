using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BarCode
    {
        [DataMember, ValidationInterceptor]
        public string BarCodeData { get; set; }

        [DataMember, ValidationInterceptor]
        public BarCodeType BarCodeType { get; set; }

        [DataMember, ValidationInterceptor]
        public string BarCodeImageBase64 { get; set; }
    }
}
