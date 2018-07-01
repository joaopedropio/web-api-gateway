using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace WebAPIGateway
{
    [Route("/admin/{*service}")]
    public class AdminController: Controller
    {
        IDistributedCache cache;
        public AdminController(IDistributedCache cache)
        {
            this.cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmin(string service)
        {
            if(service == null || service == string.Empty)
            {
                return new JsonResult(new { error = "No service provided" });
            }

            JsonResult json;
            string url;
            try
            {
                url = await cache.GetStringAsync(service);
            }
            catch (System.Exception ex)
            {
                json = new JsonResult(new { error = ex.Message });
                json.StatusCode = 400;
                return json;
            }

            if (url == null || url == string.Empty)
            {
                json = new JsonResult(new { error = "Service not found" });
                json.StatusCode = 404;
                return json;
            }

            return new JsonResult(new {service, url});
        }

        [HttpPost]
        public async Task<IActionResult> PostAdmin(string service)
        {
            var content = new StreamReader(Request.Body).ReadToEnd();
            var serviceUrl = JsonConvert.DeserializeObject<ServiceUrl>(content);

            try
            {
                try
                {
                    await cache.SetStringAsync(service, serviceUrl.ToString());
                }
                catch (System.Exception ex)
                {
                    Json(new { error = ex.Message});
                }

                return new JsonResult(new { status = "Service added" });
            }
            catch (System.Exception)
            {
                var json = new JsonResult(new { error = "Body can't be empty" });
                json.StatusCode = 400;
                return json;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin(string service)
        {
            if(service == null || service == string.Empty)
            {
                return Json(new { error = "Service can not be empty" });
            }

            string value;
            try
            {
                value = await cache.GetStringAsync(service);
            }
            catch (System.Exception ex)
            {
                return Json(new { error = ex.Message});
            }

            if(value == null || value == string.Empty)
            {
                return Json(new { error = "Service does not exist" });
            } 
            else
            {
                try
                {
                    await cache.RemoveAsync(service);
                }
                catch (System.Exception ex)
                {
                    return Json(new { error = ex.Message});
                }
                return Json(new { status = "Service deleted" });
            }
        }
    }
}