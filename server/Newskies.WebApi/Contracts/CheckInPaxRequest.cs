using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInPaxRequest
    {
        //[DataMember, ValidationInterceptor]
        public Name Name { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string VerifiedDocs { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool VerifiedID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public long PassengerID { get; set; }

        //[DataMember, ValidationInterceptor]
        //public string IATCISeatPreference { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool ProcessAPPS { get; set; }

        //[DataMember, ValidationInterceptor]
        //public APPSTransitType APPSTransitType { get; set; }

        [DataMember, ValidationInterceptor]
        public long PassengerNumber { get; set; }
    }
}