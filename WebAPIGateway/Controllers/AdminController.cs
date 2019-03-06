using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebAPIGateway.Domain;
using WebAPIGateway.Helpers;

namespace WebAPIGateway
{
    [Produces("application/json")]
    [Route("/admin/{*serviceName}")]
    [ApiController]
    public class AdminController: Controller
    {
        IServiceRepository serviceRepo;
        public AdminController(IServiceRepository serviceRepo)
        {
            this.serviceRepo = serviceRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmin(string serviceName)
        {
            var json = await ServiceValidations.GetService(serviceRepo, serviceName);
            return JsonResultHelper.Parse(json);
        }

        [HttpPost]
        public async Task<IActionResult> PostAdmin()
        {
            var service = Service.ParseService(Request.Body);
            var json = await ServiceValidations.PostService(serviceRepo, service);
            return JsonResultHelper.Parse(json);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin(string serviceName)
        {
            if(string.IsNullOrEmpty(serviceName))
            {
                return new JsonResult(new { error = "Service can not be empty" })
                {
                    StatusCode = HttpStatusCode.BadRequest.GetHashCode()
                };
            }

            IService service;
            try
            {
                service = await serviceRepo.RetrieveAsync(serviceName);
            }
            catch (System.Exception ex)
            {
                return new JsonResult(new { error = ex.Message })
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode()
                };
            }

            if(string.IsNullOrEmpty(service.URL))
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
                    await serviceRepo.RemoveAsync(serviceName);
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