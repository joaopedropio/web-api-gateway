using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace WebAPIGateway.Controllers {
    
    [Route("/{*uri}")]
    public class ForwardController : Controller 
    {
        private IDistributedCache _cache;
        public ForwardController(IDistributedCache cache)
        {
            cache.SetString("user", "http://localhost:3000");
            _cache = cache;
        }

        public string Index() 
        {
            var uri = Request.Path.Value;
            var service = ParseServiceFromRequest(uri);
            var serviceDomain = GetServiceURL(service);
            
            if(serviceDomain == null || serviceDomain == "")
                return "There is no such service, you dumbass!";
            else
                return APIResponse(serviceDomain + ParseServiceURI(uri));
        }

        private string ParseServiceFromRequest(string uri)
        {
            return uri.Split('/')[1];
        }

        private string GetServiceURL(string service)
        {
            return _cache.GetString(service);
        }

        private string ParseServiceURI(string uri)
        {
            var service = ParseServiceFromRequest(uri);
            var serviceURI = uri.Remove(0, service.Length + 1);            
            return serviceURI;
        }

        private string APIResponse(string uri)
        {
            var httpRequest = HttpWebRequest.CreateHttp(uri);
            
            using(var httpResponse = httpRequest.GetResponse()){

                var stream = httpResponse.GetResponseStream();
                var response = new StreamReader(stream);
                var result = response.ReadToEnd();
                return result;
    
            }
        }
    }
}