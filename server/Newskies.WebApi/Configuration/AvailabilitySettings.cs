namespace Newskies.WebApi.Configuration
{
    public class AvailabilitySettings
    {
        public int MinHoursBetweenJourneys { get; set; }
        public string[] FareTypeCodes { get; set; }
        public bool DisableDefaultDepartStationCurrencyValidation { get; set;}
    }
}
