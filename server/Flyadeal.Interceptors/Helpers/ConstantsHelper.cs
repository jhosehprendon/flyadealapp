using System;

namespace Flyadeal.Interceptors.Helpers
{
    public static class Constants
    {
        // Payment methods codes
        internal static readonly string PaymentMethodCodeSadadOffline = "SD";
        internal static readonly string PaymentMethodCodeSadadOnline = "SO";
        internal static readonly string PaymentMethodCodeVisa = "V1";
        internal static readonly string PaymentMethodCodeMastercard = "M1";
        internal static readonly string PaymentMethodCodeVoucher = "VO";
        internal static readonly string PaymentMethodCodeAgency = "AG";
        internal static readonly string[] CardPaymentMethodCodes = new[] { PaymentMethodCodeMastercard, PaymentMethodCodeVisa };
        internal static readonly string[] AmonymousAllowedPaymentMethods = new[] { PaymentMethodCodeSadadOffline, /*PaymentMethodCodeSadadOnline,*/ PaymentMethodCodeMastercard, PaymentMethodCodeVoucher, PaymentMethodCodeVisa };
        internal static readonly string[] AgencyAllowedPaymentMethods = new[] { PaymentMethodCodeAgency, PaymentMethodCodeMastercard, PaymentMethodCodeVisa };
        internal static readonly string[] CorporateMasterAllowedPaymentMethods = new[] { PaymentMethodCodeAgency, PaymentMethodCodeVoucher, PaymentMethodCodeMastercard, PaymentMethodCodeVisa, /*PaymentMethodCodeSadadOnline,*/ PaymentMethodCodeVoucher };
        internal static readonly string[] CorporateSubAllowedPaymentMethods = new[] { PaymentMethodCodeVoucher, PaymentMethodCodeMastercard, PaymentMethodCodeVisa, /*PaymentMethodCodeSadadOnline,*/ PaymentMethodCodeVoucher };

        // Fee codes
        internal static readonly string FeeCodeSMS = "SMSF";
        internal static readonly string[] FeeCodesAllowed = new[] { FeeCodeSMS };

        // Web Check In
        internal static readonly TimeSpan CheckInMinimumPriorToDeparture = TimeSpan.FromHours(1);
        internal static readonly TimeSpan CheckInMaxumumPriorToDeparture = TimeSpan.FromHours(48);
        internal static readonly string WebCheckInBookingQueueCode = "WBCKIN";

        // Agents
        internal static readonly string AnonymousAgentRoleCode = "WWW2";
        internal static readonly string AgentSubRoleCode = "TAGT";
        internal static readonly string AgentMasterRoleCode = "TMST";
        internal static readonly string CorporateSubRoleCode = "COAG";
        internal static readonly string CorporateMasterRoleCode = "COMA";
        internal static readonly string CorporateFlexProductClass = "CF";

        // Misc
        internal static readonly string ItineraryBookingQueueCode = "ITINQ";
        internal static readonly string Free15BagSSRCode = "F15";
        internal static readonly string Free25BagSSRCode = "F25";
        internal static readonly string MemberRoleCode = "WWMB";
        internal static readonly string DomainCode = "WW2";
        internal static readonly string DepartmentCode = "WWW";
        internal static readonly string LocationCode = "WWW";
        internal static readonly short PageSize = 50;
    }
}
