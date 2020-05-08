using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.HostedServices.BackgroundServices;
using AspNetCore.HostedServices.Core.Services;
using AspNetCore.HostedServices.EF;
using AspNetCore.HostedServices.Infrastructure.Services;
using AspNetCore.HostedServices.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.HostedServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var cacheSettings = new CacheSettings();
            Configuration.GetSection(nameof(CacheSettings)).Bind(cacheSettings);
            services.AddSingleton(cacheSettings);
            
            services.AddStackExchangeRedisCache(op => op.Configuration = cacheSettings.ConnectionString);

            services.AddDbContext<salesdbContext>();

            services.AddHostedService<StudentsCacheService>();

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
