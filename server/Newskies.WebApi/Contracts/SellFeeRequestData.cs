using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class SellFeeRequestData
    {
        [DataMember, StringLength(15, MinimumLength = 3), ValidationInterceptor]
        public string FeeCode { get; set; }

        //[DataMember, ValidationInterceptor]
        public string OriginatingStationCode { get; set; }

        //[DataMember, ValidationInterceptor]
        public string CollectedCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public long PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string Note { get; set; }

        //[DataMember, ValidationInterceptor]
        public SellFeeType SellFeeType { get; set; }
    }
}
