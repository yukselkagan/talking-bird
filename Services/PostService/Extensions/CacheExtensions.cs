using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace PostService.Extensions
{
    public static class CacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId,
            T data, TimeSpan? absoluteExpirationTime = null, 
            TimeSpan? slidingExpirationTime = null)
        {

            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpirationTime ?? TimeSpan.FromSeconds(3);
            options.SlidingExpiration = slidingExpirationTime ?? TimeSpan.FromSeconds(1);

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if(jsonData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }


    }
}
