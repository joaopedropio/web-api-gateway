using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;

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

        [HttpGet]
        public async Task<JObject> ForwardGet(string service, string uri) {
            string serviceUrl = await _cache.GetStringAsync(service);

            if(serviceUrl == null || serviceUrl == "")
                return JObject.Parse("{ \"status\": \"There is no such service, you dumbass!\" }");
            else
                return JObject.Parse(await _client.GetStringAsync(serviceUrl + "/" + uri));
        }

        [HttpPost]
        public async Task<JObject> ForwardPost(string service, string uri)
        {
            string serviceUrl = await _cache.GetStringAsync(service);

            string body = await GetBodyAsync(Request);

            if(serviceUrl == null || serviceUrl == "") {
                return JObject.Parse("{ \"status\": \"There is no such service, you dumbass!\" }");
            } else {
                return await PostBodyAsync(serviceUrl + "/" + uri, body);
            }
        }

        public async Task<JObject> PostBodyAsync(string url, string body)
        {
                var content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
                var response = await _client.PostAsync(url, content);
                return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GetBodyAsync(HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {  
                return await reader.ReadToEndAsync();
            }
        }
    }
}