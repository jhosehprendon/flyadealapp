using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Agent
    {
        [DataMember, ValidationInterceptor]
        public long AgentID { get; set; }

        //[DataMember, ValidationInterceptor]
        public AgentIdentifier AgentIdentifier { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public string LoginName { get; set; }

        [DataMember, ValidationInterceptor]
        public AgentStatus Status { get; set; }

        [DataMember, ValidationInterceptor]
        public string Password { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime PasswordChangedDate { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime LogonDateTime { get; set; }

        [DataMember, ValidationInterceptor]
        public long PersonID { get; set; }

        //[DataMember, ValidationInterceptor]
        public string DepartmentCode { get; set; }

        //[DataMember, ValidationInterceptor]
        public string LocationCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string LocationGroupCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string OrganizationGroupCode { get; set; }

        //public DateTime HireDate { get; set; }

        //public DateTime TerminationDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string AgentNote { get; set; }

        //public DateTime LastAlertReadDate { get; set; }

        //public DateTime LastNewsReadDate { get; set; }

        public bool ForcePasswordReset { get; set; }

        [DataMember, ValidationInterceptor]
        public bool Locked { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string TraceQueueCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short FailedLogons { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool Allowed { get; set; }

        [DataMember, ValidationInterceptor]
        public AgentRole[] AgentRoles { get; set; }

        //public AgentSetting[] AgentSettings { get; set; }
    }
}
