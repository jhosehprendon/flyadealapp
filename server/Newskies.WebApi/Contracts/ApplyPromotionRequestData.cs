using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class ApplyPromotionRequestData
    {
        [DataMember, Required, ValidationInterceptor]
        public string PromotionCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PointOfSale SourcePointOfSale { get; set; }
    }
}
