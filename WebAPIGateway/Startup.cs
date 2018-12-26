using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebAPIGateway
{
    public class Startup
    {
        public Configuration Configuration { get; }
        public Startup()
        {
            Configuration = new Configuration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDistributedRedisCache(option => {
                option.Configuration = Configuration.DBServer;
                option.InstanceName = "master";
            });
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IDistributedCache cache)
        {
            RedisCache.DefaultValues(cache);
            app.UseDeveloperExceptionPage();

            loggerFactory.AddConsole(Configuration.Logging);
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
