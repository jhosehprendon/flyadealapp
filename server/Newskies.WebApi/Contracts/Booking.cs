using System.Runtime.Serialization;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts.Enumerations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Booking
    {
        [DataMember, ValidationInterceptor]
        public string RecordLocator { get; set; }

        [DataMember, ValidationInterceptor]
        public string CurrencyCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short PaxCount { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long BookingID { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingInfo BookingInfo { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PointOfSale POS { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingPointOfSale SourceBookingPOS { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Booking.TypeOfSale TypeOfSale { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingHold BookingHold { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingSum BookingSum { get; set; }

        [DataMember, ValidationInterceptor]
        public Passenger[] Passengers { get; set; }

        [DataMember, ValidationInterceptor]
        public Journey[] Journeys { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingComment[] BookingComments { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingContact[] BookingContacts { get; set; }

        [DataMember, ValidationInterceptor]
        public Payment[] Payments { get; set; }

        [DataMember, ValidationInterceptor]
        public MessageState State { get; set; }
    }
}
