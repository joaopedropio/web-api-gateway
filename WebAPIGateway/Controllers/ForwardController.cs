using static System.Net.HttpStatusCode;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static WebAPIGateway.Helpers.HttpResponse;
using System.IO;
using System.Text;
using WebAPIGateway.Domain;
using WebAPIGateway.JSON;

namespace WebAPIGateway.Controllers
{

    [Route("/{serviceName}/{*uri}")]
    public class ForwardController : Controller
    {
        private IServiceRepository services;
        private HttpClient client;
        public ForwardController(IServiceRepository services)
        {
            this.services = services;
            this.client = new HttpClient();
        }

        public async Task<IActionResult> Index(string serviceName, string uri)
        {
            IService service;
            string url;
            try
            {
                service = await services.RetrieveAsync(serviceName);
                url = $"{service.URL}/{uri}";
            }
            catch (Exception ex)
            {
                return JsonResultHelper.CreateUnknownErrorJsonResult(ex.Message, ex.StackTrace, InternalServerError);
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
                    return JsonResultHelper.CreateErrorJsonResult(Messages.MethodNotSupported, MethodNotAllowed);
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
                return Response(ex.Message, ServiceUnavailable);
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
                return Response(ex.Message, ServiceUnavailable);
            }
        }
    }
}