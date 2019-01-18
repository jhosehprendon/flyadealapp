using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newskies.WebApi.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flyadeal.Interceptors.Helpers
{
    public static class ValidationHelper
    {
        internal static ValidationResult ValidateCurrencyWithDepartureStation(string currencyCode,
            string departureStation, Dictionary<string, string[]> currencyOverrideDict, ValidationContext context)
        {
            currencyCode = currencyCode.ToUpper().Trim();
            if (string.IsNullOrEmpty(currencyCode))
                return new ValidationResult("Currency code required.");
            if (currencyOverrideDict.ContainsKey(currencyCode))
            {
                var currencyOverride = Array.Find(currencyOverrideDict[currencyCode],
                    p => p.Equals(departureStation, StringComparison.OrdinalIgnoreCase));
                if (currencyOverride != null)
                    return ValidationResult.Success;
            }
            foreach (var key in currencyOverrideDict.Keys)
            {
                var currencyOverride = Array.Find(currencyOverrideDict[key], p => p.Equals(departureStation, StringComparison.OrdinalIgnoreCase));
                if (currencyOverride != null)
                    return new ValidationResult(string.Format(
                        "Invalid currency code {0} in availability request for departing station {1}. Override settings indicate it must be {2}.",
                        currencyCode, departureStation, key));
            }

            // do default currency code validation
            return Newskies.WebApi.Helpers.ValidationHelper.DefaultCurrencyValidation(currencyCode, context);
        }

        internal static ValidationResult ProcessAvailabilityRequestArrayValidation(object value, ValidationContext validationContext, Dictionary<string, string> settings)
        {
            var avRequests = value as AvailabilityRequest[];
            if (avRequests != null)
            {
                // validate that currency code of first journey (only) is valid as per currency override configuration.
                var currencyCode = avRequests[0].CurrencyCode;
                var departureStation = avRequests[0].DepartureStation;
                var currencyOverrideDict = SettingsHelper.ParseCurrencyOverrideSettings(settings);
                var result = ValidateCurrencyWithDepartureStation(currencyCode, departureStation, currencyOverrideDict, validationContext);
                if (result != null)
                    return result;
            }

            var requests = value as BaseAvailabilityRequest[]; // LFF and normal availability
            if (requests.Length > 1)
            {
                for (var i = 0; i < requests.Length - 1; i++)
                {
                    // validate 2nd journey origin is the same as 1st journey destination, and vicec-versa
                    // NOTE: This needs to be adjusted in future if Open Jaw flights are introduced.
                    if (requests[i].ArrivalStation != requests[i + 1].DepartureStation)
                        return new ValidationResult(string.Format("Invalid departure station. Journey{0} departure station must equal journey{1} arrival station.",
                            i + 1, i));
                    if (requests[i].DepartureStation != requests[i + 1].ArrivalStation)
                        return new ValidationResult(string.Format("Invalid arrival station. Journey{0} arrival station must equal journey{1} departure station.",
                            i, i + 1));
                }
            }

            return ValidationResult.Success;
        }

        internal static ValidationResult CardPaymentStringMemberIsValid(object value, ValidationContext validationContext, string regEx = null)
        {
            var str = value as string ?? "";
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
            {
                // validate if string member is required for the selected payment method
                var addPaymentToBookingRequestData = validationContext.ObjectInstance as AddPaymentToBookingRequestData;
                if (addPaymentToBookingRequestData == null || string.IsNullOrEmpty(addPaymentToBookingRequestData.PaymentMethodCode))
                    return ValidationResult.Success;
                if (!Constants.CardPaymentMethodCodes.ToList().Contains(addPaymentToBookingRequestData.PaymentMethodCode))
                    return ValidationResult.Success;
            }
            else
            {
                if (string.IsNullOrEmpty(regEx))
                    return ValidationResult.Success;
                if (Regex.IsMatch(str, regEx))
                    return ValidationResult.Success;
            }
            return new ValidationResult(string.Format("Invalid {0}.", validationContext.MemberName));
        }

        internal static bool MatchPaxOrContactLastName(this Booking booking, string lastName)
        {
            if (booking == null || booking.Passengers == null || string.IsNullOrEmpty(lastName) || booking.BookingContacts == null)
            {
                return false;
            }
            var paxMatched = booking.Passengers.Any(p => p.Names != null && p.Names.Any(n => n.LastName != null && lastName.Trim().Equals(n.LastName, StringComparison.OrdinalIgnoreCase)));
            if (paxMatched)
            {
                return true;
            }
            var contactMatched = booking.BookingContacts.Any(c => c.Names != null && c.Names.Any(n => n.LastName != null && lastName.Trim().Equals(n.LastName, StringComparison.OrdinalIgnoreCase)));
            return contactMatched;
        }

        internal static bool IsWebCheckInAllowed(this Segment segment)
        {
            return segment.PaxSegments != null && segment.PaxSegments
                .Any(ps => ps.CheckInStatus.HasValue && ps.CheckInStatus.Value == PaxSegmentCheckInStatus.Allowed);
        }

        public static bool ChangesNotAllowed(this Segment segment)
        {
            return segment.PaxSegments != null && segment.PaxSegments.Any(
                p => p.CheckInStatus.HasValue &&
                p.CheckInStatus.Value == PaxSegmentCheckInStatus.AlreadyCheckedIn ||
                p.CheckInStatus.Value == PaxSegmentCheckInStatus.FlightHasAlreadyDeparted ||
                p.CheckInStatus.Value == PaxSegmentCheckInStatus.TooCloseToDeparture);
        }

        internal static int GetAge(DateTime reference, DateTime birthday)
        {
            int age = reference.Year - birthday.Year;
            if (new DateTime(reference.Year, reference.Month, reference.Day) < birthday.AddYears(age)) age--;
            return age;
        }

        internal static bool SpecialAssistanceSSRSellAllowed(Booking booking)
        {
            if (booking == null || booking.Journeys == null || booking.Journeys.Length == 0)
                return false;
            var leg = booking.Journeys[0].Segments[0].Legs[0];
            var stdUtc = leg.STD.AddMinutes(-leg.LegInfo.DeptLTV);
            return (stdUtc - DateTime.UtcNow).TotalHours > 24;
        }

        internal static string NameRegexString
        {
            get => "^[a-zA-Z]*$";

            // [mv] Was going to use this regex to allow Arabic chars as per FADIBE-176, but Travel Docs does not allow Arabic chars. 
            //get => "^([^0-9^!@#$%^&*()~\\[\\]\\\\{}|;':\",./<>?]*)$";
        }

        internal static string CityRegexString => "^[a-zA-Z\\s]*$";

        internal static string EmailRegexString => "^[A-Za-z0-9&\'._% +-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,24}$";

        internal static string PasswordRegexString => "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$\\-_%\\^&\\*])[^~,\\.]*$";

        internal static int AdultMinAge => 12;
        internal static int AdultMaxAge => 120;

        internal static int ChildMinAge => 2;
        internal static int ChildMaxAge => 11;

        internal static int InfantMinAge => 0;
        internal static int InfantMaxAge => 2;
        internal static int InfantMinDaysOld => 8;

        internal static bool IsRoleCodeForNewAgentValid(string sessionRoleCode, string inputRoleCode)
        {
            if (sessionRoleCode == Constants.AnonymousAgentRoleCode)
            {
                return inputRoleCode == Constants.MemberRoleCode;
            }
            if (sessionRoleCode == Constants.AgentMasterRoleCode)
            {
                return inputRoleCode == Constants.AgentSubRoleCode || inputRoleCode == Constants.AgentMasterRoleCode;
            }
            if (sessionRoleCode == Constants.CorporateMasterRoleCode)
            {
                return inputRoleCode == Constants.CorporateSubRoleCode || inputRoleCode == Constants.CorporateMasterRoleCode;
            }
            return false;
        }
    }
}
