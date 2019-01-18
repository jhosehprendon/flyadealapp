using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class CheckInPaxResponse
    {
        [DataMember, ValidationInterceptor]
        public long PassengerID { get; set; }

        [DataMember, ValidationInterceptor]
        public Name Name { get; set; }

        [DataMember, ValidationInterceptor]
        public CheckInError[] ErrorList { get; set; }

        [DataMember, ValidationInterceptor]
        public string[] WarningList { get; set; }

        [DataMember, ValidationInterceptor]
        public string ConnectingMsg { get; set; }

        [DataMember, ValidationInterceptor]
        public bool PromptForSameDayReturnSeatAssign { get; set; }

        [DataMember, ValidationInterceptor]
        public bool IsSelecteePax { get; set; }

        [DataMember, ValidationInterceptor]
        public short BoardingZone { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxBPPR { get; set; }

        [DataMember, ValidationInterceptor]
        public string InfBPPR { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Operation.PassengerIATCIResponse[] PassengerIATCIResponse { get; set; }

        [DataMember, ValidationInterceptor]
        public string PaxIAPPClearance { get; set; }

        [DataMember, ValidationInterceptor]
        public string InfIAPPClearance { get; set; }

        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Operation.APPSBoardingDirective[] APPSBoardingDirectiveList { get; set; }
        //[DataMember, ValidationInterceptor]
        //public Navitaire.WebServices.DataContracts.Operation.SecurityAuthorization[] SecurityAuthorizationList { get; set; }
    }
}