using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace WebAPIGateway.Controllers {
    
    [Route("/{service}/{*uri}")]
    public class ForwardController : Controller 
    {
        private IDistributedCache _cache;
        private HttpClient _client = new HttpClient();
        public ForwardController(IDistributedCache cache)
        {
            cache.SetString("user", "http://localhost:3000");
            _cache = cache;
        }

        public async Task<string> Index(string service, string uri)
        {
            string serviceUrl = await _cache.GetStringAsync(service);

            if(serviceUrl == null || serviceUrl == "")
                return "There is no such service, you dumbass!";
            else
                return await _client.GetStringAsync(serviceUrl + "/" + uri);
        }
    }
}