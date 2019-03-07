using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPIGateway.JSON
{
    public static class JsonResultHelper
    {
        public static IActionResult Parse(IJsonResponse json)
        {
            return new JsonResult(json.Data)
            {
                StatusCode = json.StatusCode.GetHashCode()
            };
        }

        public static IActionResult CreateErrorJsonResult(string error, HttpStatusCode statusCode)
        {
            var data = new ErrorJsonData(error);
            var json = new JsonResponse(data, statusCode);
            return Parse(json);
        }

        public static IActionResult CreateUnknownErrorJsonResult(string error, string stackTrace, HttpStatusCode statusCode)
        {
            var data = new UnknownErrorJsonData(error, stackTrace);
            var json = new JsonResponse(data, statusCode);
            return Parse(json);
        }
    }
}
