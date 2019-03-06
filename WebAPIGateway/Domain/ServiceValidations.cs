using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPIGateway.Helpers;

namespace WebAPIGateway.Domain
{
    public static class ServiceValidations
    {
        public static async Task<JsonResponse> GetService(IServiceRepository serviceRepo, string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                return new JsonResponse(new { error = "No service provided" }, HttpStatusCode.BadRequest);
            }

            IService service;
            try
            {
                service = await serviceRepo.RetrieveAsync(serviceName);
            }
            catch (Exception ex)
            {
                return new JsonResponse(new { error = ex.Message }, HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(service.URL))
            {
                return new JsonResponse(new { error = "Service not found" },  HttpStatusCode.NotFound);
            }

            return new JsonResponse(service, HttpStatusCode.OK);
        }
    }
}
