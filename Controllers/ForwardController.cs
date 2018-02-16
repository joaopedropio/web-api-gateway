using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using ParseStream;
using Service;
using JSON;

namespace WebAPIGateway.Controllers
{

    [Route("/{service}/{*uri}")]
    public class ForwardController : Controller 
    {
        private IDistributedCache _cache;
        private HttpClient _client = new HttpClient();
        public ForwardController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> ForwardGet(string service, string uri)
        {
            string url = await _cache.GetServiceAsync(service, uri);
            string response = await _client.GetStringAsync(url);
            JContainer res;
            try
            {
                res = response.toJSONArray();
            }
            catch
            {
                res = response.toJSON();
            }
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ForwardPost(string service, string uri)
        {
            string url = await _cache.GetServiceAsync(service, uri);

            string body = Request.Body.toString();

            return await PostBodyAsync(url, body);
        }

        [HttpDelete]
        public async Task<IActionResult> ForwardDelete(string service, string uri)
        {
            string url = await _cache.GetServiceAsync(service, uri);
            await _client.DeleteAsync(url);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ForwardPut(string service, string uri)
        {
            string url = await _cache.GetServiceAsync(service, uri);
            string body = Request.Body.toString();
            return await PutBodyAsync(url, body);
        }

        public async Task<IActionResult> PutBodyAsync(string url, string body)
        {
            var content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, content);
            return Ok(JObject.Parse(await response.Content.ReadAsStringAsync()));
        }

        public async Task<IActionResult> PostBodyAsync(string url, string body)
        {
            var content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            return Created(url, JObject.Parse(await response.Content.ReadAsStringAsync()));
        }
    }
}