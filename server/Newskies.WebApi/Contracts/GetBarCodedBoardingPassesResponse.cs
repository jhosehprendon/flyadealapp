using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetBarCodedBoardingPassesResponse
    {
        [DataMember, ValidationInterceptor]
        public BarCodedBoardingPass[] BarCodedBoardingPasses { get; set; }

        [DataMember, ValidationInterceptor]
        public int BarcodeWidth { get; set; }

        [DataMember, ValidationInterceptor]
        public int BarcodeHeight { get; set; }
    }
}
