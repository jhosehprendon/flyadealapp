using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Contracts
{
    public class StationsMarkets
    {
        public StationSimplified[] StationsList { get; set; }
        public MarketSimplified[] MarketsList { get; set; }
    }

    public class MarketSimplified
    {
        public string Key { get; set; }
        public string[] Value { get; set; }
    }

    public class StationSimplified
    {
        public string Code { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
    }
}
