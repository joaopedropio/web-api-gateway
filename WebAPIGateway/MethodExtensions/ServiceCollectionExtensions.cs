using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIGateway.MethodExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCache(this IServiceCollection services)
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
    }
}
