using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class AllResourcesResponse
    {
        [DataMember, ValidationInterceptor]
        public Station[] StationList { get; set; }
        [DataMember, ValidationInterceptor]
        public Market[] MarketList { get; set; }
        [DataMember, ValidationInterceptor]
        public Culture[] CultureList { get; set; }
        [DataMember, ValidationInterceptor]
        public Currency[] CurrencyList { get; set; }
        [DataMember, ValidationInterceptor]
        public PaxType[] PaxTypeList { get; set; }
        [DataMember, ValidationInterceptor]
        public SSR[] SSRList { get; set; }
        [DataMember, ValidationInterceptor]
        public Fee[] FeeList { get; set; }
        [DataMember, ValidationInterceptor]
        public DocType[] DocTypeList { get; set; }
        [DataMember, ValidationInterceptor]
        public Title[] TitleList { get; set; }
        [DataMember, ValidationInterceptor]
        public Country[] CountryList { get; set; }
        [DataMember, ValidationInterceptor]
        public PaymentMethod[] PaymentMethodList { get; set; }
    }
}