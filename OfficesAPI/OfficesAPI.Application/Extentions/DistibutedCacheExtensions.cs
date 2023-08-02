using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace OfficesAPI.Application.Extentions
{
    internal static class DistibutedCacheExtensions
    {
        public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken) where T: class
        {
            var jsonObject = await cache.GetStringAsync(key, cancellationToken);

            return jsonObject == null ? default : JsonSerializer.Deserialize<T>(jsonObject);
        }

        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken) where T : class
        {
            var jsonObject = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            };
            await cache.SetStringAsync(key,jsonObject, options, cancellationToken);
        }
    }
}
