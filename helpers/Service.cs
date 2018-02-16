using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Service
{
    public static class Service
    {
        public static async Task<string> GetServiceAsync(this IDistributedCache cache, string service, string uri)
        {
            string serviceUrl = await cache.GetStringAsync(service);
            if(serviceUrl == null)
            {
                return null;
            }
            else
            {
                return serviceUrl + "/" + uri;
            }
        }
    }
}