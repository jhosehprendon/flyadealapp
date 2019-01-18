using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Account
    {
        //[DataMember, ValidationInterceptor]
        //public long AccountID { get; set; }

        [DataMember, ValidationInterceptor]
        public AccountType AccountType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Newskies.AccountManager.AccountHolderType AccountHolderType { get; set; }

        [DataMember, ValidationInterceptor]
        public AccountStatus Status { get; set; }

        [DataMember, ValidationInterceptor]
        public string AccountReference { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string Password { get; set; }

        //[DataMember, ValidationInterceptor]
        //public decimal Limit { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long PersonID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string PersonName { get; set; }

        [DataMember, ValidationInterceptor]
        public string ForeignCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal ForeignAmount { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Newskies.AccountManager.AccountCredit[] AccountCredits { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal AvailableCredits { get; set; }

        [DataMember, ValidationInterceptor]
        public string SpoiledCurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public decimal SpoiledForeignAmount { get; set; }
    }
}
