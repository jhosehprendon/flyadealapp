using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PassengerFee
    {
        [DataMember, ValidationInterceptor]
        public string ActionStatusCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string FeeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string FeeDetail { get; set; }

        [DataMember, ValidationInterceptor]
        public short FeeNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public FeeType FeeType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool FeeOverride { get; set; }

        [DataMember, ValidationInterceptor]
        public string FlightReference { get; set; }

        [DataMember, ValidationInterceptor]
        public string Note { get; set; }

        [DataMember, ValidationInterceptor]
        public string SSRCode { get; set; }

        [DataMember, ValidationInterceptor]
        public short SSRNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public short PaymentNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public BookingServiceCharge[] ServiceCharges { get; set; }

        //[DataMember, ValidationInterceptor]
        //public DateTime CreatedDate { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool IsProtected { get; set; }

        [DataMember, ValidationInterceptor]
        public FeeApplicationType FeeApplicationType { get; set; }
    }
}
