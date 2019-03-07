using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIGateway.Domain
{
    public class ServiceRedisRepository : IServiceRepository
    {
        private IDatabase redis;

        public ServiceRedisRepository(IDatabase cache)
        {
            this.redis = cache;
        }

        public ServiceRedisRepository(IDatabase cache, IList<IService> defaultServices)
            : this(cache)
        {
            defaultServices.Select(ds => this.StoreAsync(ds));
        }

        public async Task RemoveAsync(string serviceName)
        {
            var service = await this.RetrieveAsync(serviceName);
            if (service == null)
                return;

            await redis.SetRemoveAsync(service.Name, service.URL);
        }

        public async Task<IService> RetrieveAsync(string serviceName)
        {
            var serviceUrl = await Task.Run(() => redis.StringGetAsync(serviceName));
            return new Service(serviceName, serviceUrl);
        }

        public async Task StoreAsync(IService service)
        {
            await redis.StringSetAsync(service.Name, service.URL);
        }
    }
}
