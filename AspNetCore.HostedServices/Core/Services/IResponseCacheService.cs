using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HostedServices.Core.Services
{
    public interface IResponseCacheService
    {
        Task CacheReponse(string key, object value, TimeSpan time);

        Task<string> GetCachedData(string Key);
    }
}
