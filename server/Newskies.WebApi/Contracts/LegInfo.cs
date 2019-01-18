using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LegInfo
    {
        [DataMember, ValidationInterceptor]
        public string EquipmentType { get; set; }

        [DataMember, ValidationInterceptor]
        public string EquipmentTypeSuffix { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalTerminal { get; set; }

        [DataMember, ValidationInterceptor]
        public string CodeShareIndicator { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureTerminal { get; set; }

        [DataMember, ValidationInterceptor]
        public bool ETicket { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime PaxSTA { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime PaxSTD { get; set; }

        [DataMember, ValidationInterceptor]
        public string ScheduleServiceType { get; set; }

        [DataMember, ValidationInterceptor]
        public string OperatingFlightNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string OperatedByText { get; set; }

        [DataMember, ValidationInterceptor]
        public string OperatingCarrier { get; set; }

        [DataMember, ValidationInterceptor]
        public string OperatingOpSuffix { get; set; }

        [DataMember, ValidationInterceptor]
        public bool SubjectToGovtApproval { get; set; }

        [DataMember, ValidationInterceptor]
        public short ArrvLTV { get; set; }

        [DataMember, ValidationInterceptor]
        public short DeptLTV { get; set; }

    }
}