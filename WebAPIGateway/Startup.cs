using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace WebAPIGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            ConfigureCache(services);
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDistributedCache cache)
        {
            LoadDefaultServices(cache, Configuration.Services);
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole(Configuration.Logging);
                loggerFactory.AddDebug();
            }

            app.UseMvc();
        }

        public void ConfigureCache(IServiceCollection services)
        {
            if (Configuration.UseRedisCache)
            {
                services.AddDistributedRedisCache(option => {
                    option.Configuration = Configuration.CacheDomain;
                    option.InstanceName = "master";
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
        }

        private void LoadDefaultServices(IDistributedCache cache, IEnumerable<Service> services)
            => services?.Select(service => cache.SetStringAsync(service.Name, service.URL));
    }
}
