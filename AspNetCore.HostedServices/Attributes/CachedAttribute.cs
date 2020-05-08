using AspNetCore.HostedServices.Core.Services;
using AspNetCore.HostedServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.HostedServices.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        public int Time { get; set; }

        public CachedAttribute(int time)
        {
            Time = time;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //Before Controller is invoked

            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<CacheSettings>();

            //if cache is not enabled
            if (cacheSettings == null || !cacheSettings.Enabled) await next();

            var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            //Get a key to for the cache entry.. generate from current url and headers
            // bcoz versioning headers can change and the cache needs to 
            // return based on headers also
            string cacheKey = GetCacheKeyFromRequest(context.HttpContext.Request);

            //Get the value for the cache
            // We have set a cache period of 10 secs and after 10 secs
            // the data becomes stale and is not returned from the API.
            var data = await responseCacheService.GetCachedData(cacheKey);

            if (!string.IsNullOrWhiteSpace(data))
            {
                context.Result = new ContentResult()
                {
                    Content = data,
                    StatusCode = 200,
                    ContentType = "application/json"
                };
                return;
            }

            //After Controller code is executed
            var response = await next();

            // Get the result from the response and cache it 
            if (response.Result is OkObjectResult okObjectResult)
            {
                await responseCacheService.CacheReponse(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(Time));
            }


            string GetCacheKeyFromRequest(HttpRequest request)
            {
                var cacheKey = new StringBuilder();

                cacheKey.Append($"{request.Path}");

                foreach (var item in request.Query.OrderBy(i => i.Key))
                {
                    cacheKey.Append($"|{item.Key}_{item.Value}");
                }

                return cacheKey.ToString();
            }
        }


    }
}
