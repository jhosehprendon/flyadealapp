using Microsoft.AspNetCore.Builder;
using System;

namespace Newskies.WebApi.Configuration
{
    public class ApplicationSessionOptions: SessionOptions
    {
        public string SessionTokenHeader { get; set; }

        public TimeSpan NewskiesIdleTimeout { get; set; }

        public string RedisHost { get; set; }

        public string RedisName { get; set; }
    }
}