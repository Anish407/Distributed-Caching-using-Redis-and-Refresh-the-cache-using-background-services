using AspNetCore.HostedServices.Core.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AspNetCore.HostedServices.Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        public ResponseCacheService(IDistributedCache distributedCache) => (DistributedCache) = (distributedCache);

        public IDistributedCache DistributedCache { get; }

        public async Task CacheReponse(string key, object value, TimeSpan time)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key cannot be null");

            await DistributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = time
            });
        }

        public async Task<string> GetCachedData(string Key) => await DistributedCache.GetStringAsync(Key);
    }
}
