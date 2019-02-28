using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIGateway
{
    public static class RedisCache
    {
        public static void DefaultValues(IDistributedCache cache)
        {
            if (Configuration.Services == null)
                return;

            //var services = ParseServices(Configuration.Services);
            foreach (var service in Configuration.Services)
            {
                cache.SetString(service.Name, service.URL);
            }
        }
    }
}
