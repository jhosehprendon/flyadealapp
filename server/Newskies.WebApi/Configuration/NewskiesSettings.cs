using System;

namespace Newskies.WebApi.Configuration
{
    public class NewskiesSettings
    {
        public ServiceEndpoints ServiceEndpoints { get; set; }
        public int ServiceTimeoutSeconds { get; set; }
        public string AgentDomain { get; set; }
        public string AnonymousAgentName { get; set; }
        public string AnonymousAgentPassword { get; set; }
        public string AnonymousAgentRole { get; set; }
        public string AnonymousAgentOrganizationCode { get; set; }
        public string DefaultAgentOrgCode { get; set; }
        public string DefaultAgentDepartmentCode { get; set; }
        public string DefaultAgentLocationCode { get; set; }
        public string DefaultAgentRoleCode { get; set; }
        public string DefaultMemberRoleCode { get; set; }
        public string DefaultCulture { get; set; }
        public int ApiContractVersion { get; set; }
        public string MsgContractVersion { get; set; }
        public TimeSpan ResourcesCachePeriod { get; set; }
        public string MasterAgentRoleCode { get; set; }
        public string DefaultCorporateRoleCode { get; set; }
        public string MasterCorporateRoleCode { get; set; }

    }
}
