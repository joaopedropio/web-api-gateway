using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPIGateway.Helpers
{
    public static class JsonResultHelper
    {
        public static JsonResult Parse(JsonResponse json)
        {
            return new JsonResult(json.Data)
            {
                StatusCode = json.StatusCode.GetHashCode()
            };
        }
    }
}
