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
    
    public class ForwardController : Controller 
    {
        private IDistributedCache _cache;
        private HttpClient _client = new HttpClient();
        public ForwardController(IDistributedCache cache)
        {
            cache.SetString("user", "http://localhost:3000");
            _cache = cache;
        }


        [Route("/{service}/{*uri}")]
        public async Task<IActionResult> Index(string service, string uri)
        {
            string serviceUrl = await _cache.GetStringAsync(service);
            if(serviceUrl == null) {
                return Ok(JObject.Parse("{ \"error\": \"Invalid service\" }"));
            }
            string url = serviceUrl + "/" + uri;

            switch(Request.Method) {
                case "GET": return await ForwardGet(url);
                case "POST": return await ForwardPost(url, Request.Body);
                case "DELETE": return await ForwardDelete(url);
                default: return Ok(JObject.Parse("{ \"status\": \"Method not valid!\" }"));
            }
        }
        public async Task<IActionResult> ForwardGet(string url) {
            string response = await _client.GetStringAsync(url);
            try {
                return Ok(JArray.Parse(response));
            }
            catch {
                return Ok(JObject.Parse(response));
            }
        }
        public async Task<IActionResult> ForwardPost(string url, Stream bodyStream)
        {
            string body = await ParseBodyAsync(bodyStream);

            return await PostBodyAsync(url, body);
        }
        public async Task<IActionResult> ForwardDelete(string url)
        {
            var response = await _client.DeleteAsync(url);
            return NoContent();
        }
        public async Task<IActionResult> PostBodyAsync(string url, string body)
        {
            var content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            return Created(url, JObject.Parse(await response.Content.ReadAsStringAsync()));
        }
        public async Task<string> ParseBodyAsync(Stream bodyStream)
        {
            using (StreamReader reader = new StreamReader(bodyStream, Encoding.UTF8))
            {  
                return await reader.ReadToEndAsync();
            }
        }
    }
}