using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIGateway
{
    public static class RedisCache
    {
        public static void DefaultValues(IDistributedCache cache, IList<Service> services)
        {
            services?.Select(service => cache.SetStringAsync(service.Name, service.URL));
        }
    }
}
