using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Newskies.WebApi.Extensions
{
    public static class SessionBagExtensions
    {
        public async static Task<string> GetCustomSessionValue(this ISessionBagService sessionBagService, string sessionKey)
        {
            var customSessionValues = await sessionBagService.CustomSessionValues();
            if (customSessionValues != null && customSessionValues.ContainsKey(sessionKey))
                return customSessionValues[sessionKey];
            return null;
        }

        public static async Task SetCustomSessionValue(this ISessionBagService sessionBagService, string sessionKey, string value)
        {
            var customSessionValues = await sessionBagService.CustomSessionValues();
            if (customSessionValues == null)
            {
                customSessionValues = new Dictionary<string, string>();
            }
            if (customSessionValues.ContainsKey(sessionKey))
            {
                customSessionValues[sessionKey] = value;
            }
            else
            {
                customSessionValues.Add(sessionKey, value);
            }
            await sessionBagService.SetCustomSessionValues(customSessionValues);
        }
    }
}
