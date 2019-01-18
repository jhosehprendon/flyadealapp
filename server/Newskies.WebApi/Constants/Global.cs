using Newskies.WebApi.Configuration;
using Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Constants
{
    public static class Global
    {
        public const string ADULT_CODE = "ADT";
        public const string INFANT_CODE = "INFT";
        public const string CHILD_CODE = "CHD";

        public static AppSettings NonSpecifiedDefaultSettings
        {
            get
            {
                return new AppSettings
                {
                    AvailabilitySettings = new AvailabilitySettings
                    {
                        FareTypeCodes = new[] { "R" },
                        MinHoursBetweenJourneys = 2,
                        DisableDefaultDepartStationCurrencyValidation = false
                    },
                    Cultures = new[] { new Culture { Code = "en-US", Name = "English - United States" } },
                    Currencies = new[] { new Currency { Code = "USD", Name = "United States Dollar", Symbol = "$" } }
                };
            }
        }
    }
}
