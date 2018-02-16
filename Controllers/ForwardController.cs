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
            if(service.ToLower() == "admin")
            {
                switch(Request.Method)
                {
                    case "GET" : return Ok(JObject.Parse(await GetAdmin(uri)));
                    case "POST": return Ok(JObject.Parse(await PostAdmin(Request.Body)));
                    case "DELETE": return Ok(JObject.Parse(await DeleteAdmin(uri)));
                    default: return Ok("deu Ruim");
                }
            }
            string serviceUrl = await _cache.GetStringAsync(service);
            if(serviceUrl == null)
            {
                return Ok(JObject.Parse("{ \"error\": \"Invalid service\" }"));
            }
            string url = serviceUrl + "/" + uri;

            switch(Request.Method)
            {
                case "GET": return await ForwardGet(url);
                case "POST": return await ForwardPost(url, Request.Body);
                case "DELETE": return await ForwardDelete(url);
                case "PUT": return await ForwardPut(url, Request.Body);
                default: return Ok(JObject.Parse("{ \"status\": \"Method not valid!\" }"));
            }
        }
        public async Task<IActionResult> ForwardGet(string url)
        {
            string response = await _client.GetStringAsync(url);
            try
            {
                return Ok(JArray.Parse(response));
            }
            catch
            {
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

        public async Task<IActionResult> ForwardPut(string url, Stream bodyStream)
        {
            string body = await ParseBodyAsync(bodyStream);

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
        public async Task<string> ParseBodyAsync(Stream bodyStream)
        {
            using (StreamReader reader = new StreamReader(bodyStream, Encoding.UTF8))
            {  
                return await reader.ReadToEndAsync();
            }
        }

        public async Task<string> GetAdmin(string service)
        {
            if(service == null)
            {
                return "{ \"error\": \"no service provided\" }";
            }
            string url = await _cache.GetStringAsync(service);
            if (url == "" || url == null)
            {
                return "{ \"error\": \"service not found\" }";
            }
            return "{\"service\": \"" + service + "\", \"url\": \"" + url + "\" }";
        }

        public async Task<string> PostAdmin(Stream bodyStream)
        {
            var bodyString = await ParseBodyAsync(bodyStream);
            JObject body = JObject.Parse(bodyString);

            try
            {
                string service = (string)body.GetValue("service");
                string url = (string)body.GetValue("url");

                await _cache.SetStringAsync(service, url);
                return "{ \"status\": \"Service Added!\" }";
            }
            catch (System.Exception)
            {
                return "{ \"error\": \"Body cant be empty\" }"; 
            }
        }

        public async Task<string> DeleteAdmin(string service)
        {
            if(service == null || service == "")
            {
                return "{ \"error\": \"servic can not be empty!\" }"; 
            }
            string value = await _cache.GetStringAsync(service);
            if(value == null || value == "")
            {
                return "{ \"error\": \"service does not exist\" }"; 
            } else {
                await _cache.RemoveAsync(service);
                return "{ \"status\": \"service deleted!\" }"; 
            }
        }
    }
}