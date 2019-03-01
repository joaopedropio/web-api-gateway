using System;
using System.IO;
using System.Net;
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
            if(string.IsNullOrEmpty(service))
            {
                return new JsonResult(new { error = "No service provided" })
                {
                    StatusCode = HttpStatusCode.BadRequest.GetHashCode()
                };
            }

            string url;
            try
            {
                url = await cache.GetStringAsync(service);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message })
                {
                    StatusCode = HttpStatusCode.BadRequest.GetHashCode()
                };
            }

            if(string.IsNullOrEmpty(url))
            {
                return new JsonResult(new { error = "Service not found" })
                {
                    StatusCode = HttpStatusCode.NotFound.GetHashCode()
                };
            }

            return new JsonResult(new { service, url })
            {
                StatusCode = HttpStatusCode.OK.GetHashCode()
            };
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
                    return new JsonResult(new { error = ex.Message })
                    {
                        StatusCode = HttpStatusCode.InternalServerError.GetHashCode()
                    };
                }

                return new JsonResult(new { status = "Service added" });
            }
            catch (System.Exception)
            {
                return new JsonResult(new { error = "Body can't be empty" })
                {
                    StatusCode = HttpStatusCode.BadRequest.GetHashCode()
                };
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin(string service)
        {
            if(string.IsNullOrEmpty(service))
            {
                return new JsonResult(new { error = "Service can not be empty" })
                {
                    StatusCode = HttpStatusCode.BadRequest.GetHashCode()
                };
            }

            string value;
            try
            {
                value = await cache.GetStringAsync(service);
            }
            catch (System.Exception ex)
            {
                return new JsonResult(new { error = ex.Message })
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode()
                };
            }

            if(string.IsNullOrEmpty(value))
            {
                return new JsonResult(new { error = "Service does not exist" })
                {
                    StatusCode = HttpStatusCode.NotFound.GetHashCode()
                };
            } 
            else
            {
                try
                {
                    await cache.RemoveAsync(service);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new { error = ex.Message })
                    {
                        StatusCode = HttpStatusCode.InternalServerError.GetHashCode()
                    };
                }

                return new JsonResult(new { status = "Service deleted" })
                {
                    StatusCode = HttpStatusCode.NoContent.GetHashCode()
                };
            }
        }
    }
}