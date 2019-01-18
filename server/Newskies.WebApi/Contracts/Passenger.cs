using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Passenger
    {
        //[DataMember, ValidationInterceptor]
        //public string CustomerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }

        //[DataMember, ValidationInterceptor]
        //public short FamilyNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxDiscountCode { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingName[] Names { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerInfant Infant { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerInfo PassengerInfo { get; set; }

        //[DataMember, ValidationInterceptor] 
        //public Navitaire.WebServices.DataContracts.Booking.PassengerProgram PassengerProgram { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerFee[] PassengerFees { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PassengerAddress[] PassengerAddresses { get; set; }

        [DataMember, ValidationInterceptor]
        public PassengerTravelDocument[] PassengerTravelDocuments { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PassengerBag[] PassengerBags { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long PassengerID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PassengerTypeInfo[] PassengerTypeInfos { get; set; }

        //[DataMember, ValidationInterceptor]
        //public PassengerInfo[] PassengerInfos { get; set; }

        // [DataMember, ValidationInterceptor]
        // public PassengerInfant[] PassengerInfants { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool PseudoPassenger { get; set; }

        [DataMember, Required, ValidationInterceptor]
        public PassengerTypeInfo PassengerTypeInfo { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Booking.PassengerEMDCoupon[] PassengerEMDCouponList { get; set; }
    }
}
