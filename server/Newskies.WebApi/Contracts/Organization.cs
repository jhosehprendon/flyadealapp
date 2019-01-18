using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{

    [DataContract]
    public class Organization
    {
        [DataMember, /*Required, StringLength(10, MinimumLength = 3),*/ ValidationInterceptor]
        public string OrganizationCode { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public OrganizationType OrganizationType { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public string OrganizationName { get; set; }

        public OrganizationStatus Status { get; set; }

        //public string ParentOrganizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        public Address Address { get; set; }

        [DataMember, ValidationInterceptor]
        public string URL { get; set; }

        [DataMember, ValidationInterceptor]
        public string Phone { get; set; }

        //public string Fax { get; set; }

        [DataMember, ValidationInterceptor]
        public string EmailAddress { get; set; }

        //public string InternalNote { get; set; }

        //public string ExternalNote { get; set; }

        [DataMember, ValidationInterceptor]
        public string CultureCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public Name ContactName { get; set; }

        [DataMember, ValidationInterceptor]
        public string ContactPhone { get; set; }

        //public string ContactOtherPhone { get; set; }

        //public System.DateTime LastStatementDate { get; set; }

        //public string StatementNote { get; set; }

        //public bool Commissionable { get; set; }

        //public bool RecalcCommission { get; set; }

        //public bool RecallCommission { get; set; }

        //public bool NettedTotal { get; set; }

        //public ExternalDistributionOption GDSEmailItinerary { get; set; }

        //public OrganizationSource Source { get; set; }

        //public OrganizationSourceStatus SourceStatus { get; set; }

        //public string TraceQueueCode { get; set; }

        //public ReferralType ReferralType { get; set; }

        //public string NewOrganizationCode { get; set; }

        //public bool SendNotification { get; set; }

        //public OrganizationProcessSchedule[] OrganizationProcessSchedules { get; set; }

        //public OrganizationExternalAccount[] OrganizationExternalAccounts { get; set; }

        //public OrganizationCommissionRate[] OrganizationCommissionRates { get; set; }
    }
}
