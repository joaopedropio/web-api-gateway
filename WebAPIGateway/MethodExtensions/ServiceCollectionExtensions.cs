using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIGateway.Domain;

namespace WebAPIGateway.MethodExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCache(this IServiceCollection services)
        {
            if (Configuration.UseRedisCache)
            {
                var redis = new Redis();
                services.AddSingleton<IServiceRepository>(new ServiceRedisRepository(redis.Instance));
            }
            else
            {
                services.AddSingleton<IServiceRepository>(new ServiceInMemoryRepository());
            }
        }
    }
}
