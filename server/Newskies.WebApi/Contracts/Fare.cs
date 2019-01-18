using Newskies.WebApi.Contracts.Enumerations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Fare
    {
        [DataMember, ValidationInterceptor]
        public string ClassOfService { get; set; }

        [DataMember, ValidationInterceptor]
        public string ClassType { get; set; }

        [DataMember, ValidationInterceptor]
        public string RuleTariff { get; set; }

        [DataMember, ValidationInterceptor]
        public string CarrierCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string RuleNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string FareBasisCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short FareSequence { get; set; }

        [DataMember, ValidationInterceptor]
        public string FareClassOfService { get; set; }

        [DataMember, ValidationInterceptor]
        public FareStatus FareStatus { get; set; }

        [DataMember, ValidationInterceptor]
        public FareApplicationType FareApplicationType { get; set; }

        [DataMember, ValidationInterceptor]
        public string OriginalClassOfService { get; set; }

        [DataMember, ValidationInterceptor]
        public string XrefClassOfService { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxFare[] PaxFares { get; set; }

        [DataMember, ValidationInterceptor]
        public string ProductClass { get; set; }

        [DataMember, ValidationInterceptor]
        public bool IsAllotmentMarketFare { get; set; }

        [DataMember, ValidationInterceptor]
        public string TravelClassCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string FareSellKey { get; set; }

        [DataMember, ValidationInterceptor]
        public InboundOutbound InboundOutbound { get; set; }

        [DataMember, ValidationInterceptor]
        public short FareLink { get; set; }

        [DataMember, ValidationInterceptor]
        public FareDesignator FareDesignator { get; set; }
    }
}