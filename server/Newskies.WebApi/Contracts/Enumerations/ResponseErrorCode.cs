using System.Runtime.Serialization;
using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum ResponseErrorCode
    {
        [EnumMember]
        InvalidRequest = 1000,

        [EnumMember]
        PromotionNotFound = 1010,

        [EnumMember]
        InvalidMarket = 1020,

        [EnumMember]
        InvalidFlightsLimit = 1030,

        [EnumMember]
        InvalidSessionToken = 1100,

        [EnumMember]
        NoBookingInSession = 1110,

        [EnumMember]
        Unauthorised = 1200,

        [EnumMember]
        NotLoggedIn = 1500,

        [EnumMember]
        InvalidLogin = 1510,

        [EnumMember]
        InvalidCurrentPassword = 1511,

        [EnumMember]
        InvalidNewPassword = 1512,

        [EnumMember]
        AlreadyLoggedIn = 1520,

        [EnumMember]
        InvalidAgentRoleCode = 1600,

        [EnumMember]
        AgentNotFound = 1610,

        [EnumMember]
        AgentAlreadyExists = 1620,

        [EnumMember]
        AgentUnauthorised = 1630,

        [EnumMember]
        AgentUpdateNotAllowed = 1640,

        [EnumMember]
        PersonUpdateUnauthorised = 1700,

        [EnumMember]
        AgencyAlreadyExists = 1800,

        [EnumMember]
        OrganizationCodeGenerationFailure = 1810,

        [EnumMember]
        BookingNotFound = 2000,

        [EnumMember]
        InvalidFee = 2200,

        [EnumMember]
        FeeNotFound = 2210,

        [EnumMember]
        ChangeFlightFailure = 2500,

        [EnumMember]
        AllSeatsAlreadyAssigned = 3000,

        [EnumMember]
        NoSeatsToUnAssign = 3001,

        [EnumMember]
        SeatAssignmentFailure = 3002,

        [EnumMember]
        SellSSRFailure = 4000,

        [EnumMember]
        PassengerDetailsUpdateNotAllowed = 3300,

        [EnumMember]
        PassengerNameUpdateNotAllowed = 3301,

        [EnumMember]
        PassengersLengthUpdateMismatch = 3302,

        [EnumMember]
        InvalidPassengerNumber = 3305,

        [EnumMember]
        TravelDocumentValidationFailure = 3310,

        [EnumMember]
        VoucherInvalid = 3400,

        [EnumMember]
        CheckInFailure = 3500,

        [EnumMember]
        InternalCommunicationsError = 4000,

        [EnumMember]
        InternalNavApiError = 5000,

        [EnumMember]
        InternalNavApiValidationError = 5100,

        [EnumMember]
        InternalException = 9000
    }
}