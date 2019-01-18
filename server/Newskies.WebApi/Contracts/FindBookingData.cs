using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class FindBookingData
    {
        [DataMember, ValidationInterceptor]
        public DateTime FlightDate { get; set; }

        [DataMember, ValidationInterceptor]
        public string FromCity { get; set; }

        [DataMember, ValidationInterceptor]
        public string ToCity { get; set; }

        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        [DataMember, ValidationInterceptor]
        public long BookingID { get; set; }

        [DataMember, ValidationInterceptor]
        public long PassengerID { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingStatus BookingStatus { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.ChannelType ChannelType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SourceOrganizationCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SourceDomainCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string SourceAgentCode { get; set; }

        [DataMember, ValidationInterceptor]
        public bool Editable { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingName Name { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime ExpiredDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool AllowedToModifyGDSBooking { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string SystemCode { get; set; }
    }
}