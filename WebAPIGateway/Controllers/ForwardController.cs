using System.Net;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using static WebAPIGateway.Helpers.HttpResponse;
using System.IO;
using System.Text;
using WebAPIGateway.MethodExtensions;

namespace WebAPIGateway.Controllers
{

    [Route("/{service}/{*uri}")]
    public class ForwardController : Controller 
    {
        private IDistributedCache cache;
        private HttpClient client;
        public ForwardController(IDistributedCache cache)
        {
            this.cache = cache;
            this.client = new HttpClient();
        }

        public async Task<ContentResult> Index(string service, string uri)
        {
            string url;
            try
            {
                url = await cache.GetServiceAsync(service, uri);
            }
            catch (Exception ex)
            {
                return new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode(),
                    Content = "{ \"error\": \"" + ex.Message + "\" }"
                };
            }

            switch (Request.Method)
            {
                case "PUT":
                    return await ForwardHttp(client.PutAsync, url);
                case "GET":
                    return await ForwardHttp(client.GetAsync, url);
                case "POST":
                    return await ForwardHttp(client.PostAsync, url);
                case "DELETE":
                    return await ForwardHttp(client.DeleteAsync, url);
                default:
                    return Response("{ \"error\": \"Method not supported\" }", HttpStatusCode.BadRequest);
            }
        }

        private async Task<ContentResult> ForwardHttp(Func<string, HttpContent, Task<HttpResponseMessage>> httpClientAsync, string url)
        {
            var content = new StringContent(new StreamReader(Request.Body).ReadToEnd(), Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClientAsync(url, content);
                return await Response(response);
            }
            catch (Exception ex)
            {
                return Response(ex.Message, HttpStatusCode.ServiceUnavailable);
            }
        }

        private async Task<ContentResult> ForwardHttp(Func<string, Task<HttpResponseMessage>> httpClientAsync, string url)
        {
            try
            {
                var response = await httpClientAsync(url);
                return await Response(response);
            }
            catch (Exception ex)
            {
                return Response(ex.Message, HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}