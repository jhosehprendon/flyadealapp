using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class PaxSeat
    {
        [DataMember, ValidationInterceptor]
        public short PassengerNumber { get; set; }

        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string UnitDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public string CompartmentDesignator { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Common.Enumerations.SeatPreference SeatPreference { get; set; }

        [DataMember, ValidationInterceptor]
        public int Penalty { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool SeatTogetherPreference { get; set; }

        [DataMember, ValidationInterceptor]
        public PaxSeatInfo PaxSeatInfo { get; set; }
    }
}
