using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace WebAPIGateway.Helpers
{
    public static class DistributedCacheExtensions
    {
        public static async Task<string> GetServiceAsync(this IDistributedCache cache, string service, string uri)
        {
            var serviceUrl = await cache.GetStringAsync(service);

            if (string.IsNullOrEmpty(serviceUrl))
                return string.Empty;

            return $"{serviceUrl}/{uri}";
        }
    }
}