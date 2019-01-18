using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerBaggageRequest
    {
        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public BaggageInfo[] BaggageInfoList { get; set; }

        public short AutoBagCount { get; set; }

        public short AutoBagWeight { get; set; }

        //public Navitaire.WebServices.DataContracts.Common.Enumerations.WeightType WeightType { get; set; }

        [DataMember, ValidationInterceptor]
        public string TaggedToStation { get; set; }

        public short CompartmentID { get; set; }

        public bool LRTIndicator { get; set; }

        public string IATAIdentifier { get; set; }

        [DataMember, ValidationInterceptor]
        public string InventoryLegKey { get; set; }
    }
}
