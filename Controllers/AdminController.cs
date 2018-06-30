using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using ParseStream;
using JSON;
using Newtonsoft.Json;

namespace WebAPIGateway
{
    [Route("/admin/{*service}")]
    public class AdminController: Controller
    {
        IDistributedCache _cache;
        public AdminController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmin(string service)
        {
            if(service == null || service == string.Empty)
            {
                return Json(new { error = "No service provided" });
            }

            string url;
            try
            {
                url = await _cache.GetStringAsync(service);
            }
            catch (System.Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            if (url == null || url == string.Empty)
            {
                return Json(new { error = "Service not found" });
            }

            return Json(new 
                {
                    service = service,
                    url = url
                });
        }

        [HttpPost]
        public async Task<IActionResult> PostAdmin(string service)
        {
            var body = Request.Body.toJSON();

            try
            {
                string url = (string)body.GetValue("url");

                try
                {
                    await _cache.SetStringAsync(service, url);
                }
                catch (System.Exception ex)
                {
                    Json(new { error = ex.Message});
                }

                return Json(new { status = "Service added" });
            }
            catch (System.Exception)
            {
                return Json(new { error = "Body can't be empty" });
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
                value = await _cache.GetStringAsync(service);
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
                    await _cache.RemoveAsync(service);
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