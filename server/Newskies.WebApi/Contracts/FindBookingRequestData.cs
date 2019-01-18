using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindBookingRequestData
    {
        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime? DepartDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string Destination { get; set; }

        [DataMember, ValidationInterceptor]
        public long AgentId { get; set; }

        [DataMember, ValidationInterceptor]
        public int LastID { get; set; }

        public FindBookingBy FindBookingBy { get; set; }

        public FindByRecordLocator FindByRecordLocator { get; set; }

        public FindByAgencyNumber FindByAgencyNumber { get; set; }

        public FindByContactCustomerNumber FindByContactCustomerNumber { get; set; }

        public short PageSize { get; set; }
    }
}
