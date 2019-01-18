using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Newskies.WebApi.Extensions
{
    public static class MemoryExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObjectAsJson(this IMemoryCache cache, string key, object value)
        {
            cache.Set(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this IMemoryCache cache, string key)
        {
            var value = cache.Get(key) as string;
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
