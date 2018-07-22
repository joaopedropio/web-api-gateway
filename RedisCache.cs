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
            var config = new Configuration();
            if (config.Services == null)
                return;

            var services = ParseServices(config.Services);
            foreach (var service in services)
            {
                cache.SetString(service.Name, service.URL);
            }
        }

        private static List<Service> ParseServices(string servicesString)
        {
            var result = new List<Service>();
            var servicesList = servicesString.Split(';');
            foreach(var name_url in servicesList)
            {
                var row = name_url.Split(',');
                var service = new Service(row[0], row[1]);
                result.Add(service);
            }
            return result;
        }
    }
}
