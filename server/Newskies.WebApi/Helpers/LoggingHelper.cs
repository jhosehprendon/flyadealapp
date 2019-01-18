using Microsoft.Extensions.Logging;

namespace Newskies.WebApi.Helpers
{
    public static class LoggingHelper
    {
        public static void WriteTimedLog<T>(this ILogger<T> logger, long msecs, string message = "")
        {
            logger.LogInformation("{0}{1}ms", !string.IsNullOrEmpty(message) ? message + "|" : "", msecs);
        }
    }
}
