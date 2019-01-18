using Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Configuration
{
    public class AppSettings
    {
        public bool EnforceSSL { get; set; }
        public bool ResponseCompression { get; set; }
        public bool CrossOriginRequests { get; set; }
        public bool IncludeServerIdHeader { get; set; }
        public ApplicationSessionOptions ApplicationSessionOptions { get; set; }
        public NewskiesSettings NewskiesSettings { get; set; }
        public PerformanceLoggingSettings PerformanceLoggingSettings { get; set; }
        public AvailabilitySettings AvailabilitySettings { get; set; }
        public PaymentSettings PaymentSettings { get; set; }
        public CommitBookingSettings CommitBookingSettings { get; set; }
        public Currency[] Currencies { get; set; }
        public Culture[] Cultures { get; set; }
    }
}
