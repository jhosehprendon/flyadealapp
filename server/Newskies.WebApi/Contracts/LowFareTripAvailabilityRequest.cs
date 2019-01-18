using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class LowFareTripAvailabilityRequest
    {
        //public bool BypassCache { get; set; }

        //public bool IncludeTaxesAndFees { get; set; }

        //public bool GroupBydate { get; set; }

        //public int ParameterSetID { get; set; }

        [DataMember, /*Currency,*/ ValidationInterceptor]
        public string CurrencyCode { get; set; }

        //public string SourceOrganizationCode { get; set; }

        [DataMember, ValidationInterceptor]
        // [Country]
        public string PaxResidentCountry { get; set; }

        [DataMember, StringLength(32), ValidationInterceptor]
        public string PromotionCode { get; set; }

        [DataMember, /*AvailabilityRequests(1, 2, 7),*/ ValidationInterceptor]
        public LowFareAvailabilityRequest[] LowFareAvailabilityRequestList { get; set; }

        //public string[] BookingClassList { get; set; }

        //public string[] ProductClassList { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] FareTypeList { get; set; }

        //public Navitaire.NewSkies.WebServices.DataContracts.Common.Enumerations.LoyaltyFilter LoyaltyFilter{ get;set;}

        //public Navitaire.NewSkies.WebServices.DataContracts.Common.Enumerations.LowFareFlightFilter FlightFilter{ get;set;}

        //public Navitaire.WebServices.DataContracts.Booking.AlternateLowFareRequest[] AlternateLowFareRequestList{ get;set;}

        //public bool GetAllDetails { get; set; }

        //public short PaxCount{ get;set;}

        //public Navitaire.WebServices.DataContracts.Booking.PaxPriceType[] PaxPriceTypeList{ get;set;}

        [DataMember, ValidationInterceptor]
        // [PaxCountInterceptor(9, 8, 9)]
        public PaxTypeCount[] PaxTypeCounts { get; set; }
    }
}
