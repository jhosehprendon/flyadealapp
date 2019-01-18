using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class BookingInfo
    {
        [DataMember, ValidationInterceptor]
        public BookingStatus BookingStatus { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string BookingType { get; set; }

        [DataMember, ValidationInterceptor]
        public ChannelType ChannelType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime CreatedDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime ExpiredDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime ModifiedDate { get; set; }

        [DataMember, ValidationInterceptor]
        public PriceStatus PriceStatus { get; set; }

        //public  Navitaire.WebServices.DataContracts.Common.Enumerations.BookingProfileStatus ProfileStatus{ get;set;}

        [DataMember, ValidationInterceptor]
        public bool ChangeAllowed { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long CreatedAgentID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long ModifiedAgentID { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime BookingDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string OwningCarrierCode { get; set; }

        [DataMember, ValidationInterceptor]
        public PaidStatus PaidStatus { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime ActivityDate { get; set; }
    }
}
