using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    public enum PaymentValidationErrorType
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        Other = 1,

        [EnumMember]
        AccountNumber = 2,

        [EnumMember]
        Amount = 3,

        [EnumMember]
        ExpirationDate = 4,

        [EnumMember]
        RestrictionHours = 5,

        [EnumMember]
        MissingAccountNumber = 6,

        [EnumMember]
        MissingExpirationDate = 7,

        [EnumMember]
        PaymentSystemUnavailable = 8,

        [EnumMember]
        MissingParentPaymentID = 9,

        [EnumMember]
        InProcessPaymentChanged = 10,

        [EnumMember]
        InvalidNumberOfInstallments = 11,

        [EnumMember]
        CreditShellCommentRequired = 12,

        [EnumMember]
        NoQuotedCurrencyProvided = 13,

        [EnumMember]
        NoBaseCurrencyForBooking = 14,

        [EnumMember]
        QuotedCurrencyDoesNotMatchBaseCurrency = 15,

        [EnumMember]
        QuotedRefundAmountNotLessThanZero = 16,

        [EnumMember]
        QuotedPaymentAmountIsLessThanZero = 17,

        [EnumMember]
        RoleCodeNotFound = 18,

        [EnumMember]
        UnknownOrInactivePaymentMethod = 19,

        [EnumMember]
        DepositPaymentsNotAllowedForPaymentMethod = 20,

        [EnumMember]
        UnableToRetrieveRoleCodeSettings = 21,

        [EnumMember]
        DepositPaymentsNotAllowedForRole = 22,

        [EnumMember]
        PaymentMethodNotAllowedForRole = 23,

        [EnumMember]
        InvalidAccountNumberLength = 24,

        [EnumMember]
        PaymentTextIsRequired = 25,

        [EnumMember]
        InvalidPaymentTextLength = 26,

        [EnumMember]
        InvalidMiscPaymentFieldLength = 27,

        [EnumMember]
        MiscPaymentFieldRequired = 28,

        [EnumMember]
        BookingCurrencyIsInvalidForSkyPay = 29,

        [EnumMember]
        SkyPayExceptionThrown = 30,

        [EnumMember]
        InvalidAccountNumberForPaymentMethod = 31,

        [EnumMember]
        InvalidELVTransaction = 32,

        [EnumMember]
        BlackListedCard = 33,

        [EnumMember]
        InvalidPaymentAddress = 34,

        [EnumMember]
        InvalidSecurityCode = 35,

        [EnumMember]
        InvalidCurrencyCode = 36,

        [EnumMember]
        InvalidAmount = 37,

        [EnumMember]
        PossibleFraud = 38,

        [EnumMember]
        InvalidCustomerAccount = 39,

        [EnumMember]
        AccountHolderIsNotAnAgency = 40,

        [EnumMember]
        InvalidStartDate = 41,

        [EnumMember]
        InvalidInitialPaymentStatus = 42,

        [EnumMember]
        PaymentCurrencyMustMatchBookingCurrency = 43,

        [EnumMember]
        CollectedAmountMustMatchPaymentAmount = 44,

        [EnumMember]
        RefundsNotAllowedUsingThisPaymentMethod = 45,

        [EnumMember]
        CreditShellAmountGreaterThanOrEqualToZero = 46,

        [EnumMember]
        CreditFileAmountLessThanOrEqualToZero = 47,

        [EnumMember]
        InvalidPrepaidApprovalCodeLength = 48,

        [EnumMember]
        AccountNumberFailedModulousCheck = 49,

        [EnumMember]
        NoExternalRatesAvailable = 50,

        [EnumMember]
        ExternalCurrencyConversion = 51,

        [EnumMember]
        InvalidVoucher = 52,

        [EnumMember]
        StoredCardSecurityViolation = 53,

        [EnumMember]
        AccountNumberDecryptionFailure = 54,

        [EnumMember]
        DirectCurrencyConversionIssueOnRefund = 55,

        [EnumMember]
        Unmapped = -1,
    }

}
