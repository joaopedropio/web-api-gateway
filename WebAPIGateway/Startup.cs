using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebAPIGateway.Domain;
using WebAPIGateway.MethodExtensions;

namespace WebAPIGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCache();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDistributedCache cache)
        {
            LoadDefaultServices(cache, Configuration.Services);
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void LoadDefaultServices(IDistributedCache cache, IEnumerable<Service> services)
            => services?.Select(service => cache.SetStringAsync(service.Name, service.URL));
    }
}
