using Newskies.WebApi.Contracts.Enumerations;

namespace Newskies.WebApi.Contracts
{
    public class OtherServiceInformation
    {
        public string Text { get; set; }

        public OSISeverity OsiSeverity { get; set; }

        public string OSITypeCode { get; set; }

        public string SubType { get; set; }
    }
}
