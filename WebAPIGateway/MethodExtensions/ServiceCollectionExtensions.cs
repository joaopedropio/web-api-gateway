using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using WebAPIGateway.Domain;

namespace WebAPIGateway.MethodExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCache(this IServiceCollection services, IList<IService> defaultServices)
        {
            if (Configuration.UseRedisCache)
            {
                var redis = new Redis();
                services.AddSingleton<IServiceRepository>(new ServiceRedisRepository(redis.Instance, defaultServices));
            }
            else
            {
                services.AddSingleton<IServiceRepository>(new ServiceInMemoryRepository(defaultServices));
            }
        }
    }
}
