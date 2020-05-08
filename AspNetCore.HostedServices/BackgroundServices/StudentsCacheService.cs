using AspNetCore.HostedServices.Core.Services;
using AspNetCore.HostedServices.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.HostedServices.BackgroundServices
{
    public class StudentsCacheService : BackgroundService
    {
        int intervalToCacheData = 5;
        string key = "/students";
        public StudentsCacheService(IResponseCacheService responseCacheService, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            ResponseCacheService = responseCacheService;
            ScopeFactory = scopeFactory;
        }

        public IResponseCacheService ResponseCacheService { get; }
        public IServiceScopeFactory ScopeFactory { get; }
        public salesdbContext SalesdbContext { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cachedData = await ResponseCacheService.GetCachedData(key);

                if (string.IsNullOrWhiteSpace(cachedData))
                {
                    //since background services are singleton and 
                    //DBcontext is scoped we cannot directly inject dbcontext
                    using (var scope = ScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<salesdbContext>();
                        var dataFromDb = await dbContext.Employees.ToListAsync();
                        await ResponseCacheService.CacheReponse(key, dataFromDb, TimeSpan.FromSeconds(10));
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(intervalToCacheData));
            }
        }
    }
}
