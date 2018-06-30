using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Service
{
    public static class Service
    {
        public static async Task<string> GetServiceAsync(this IDistributedCache cache, string service, string uri)
        {
            var serviceUrl = await cache.GetStringAsync(service);
            return serviceUrl == null ? null : string.Format($"{serviceUrl}/{uri}");
        }
    }
}