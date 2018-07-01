using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPIGateway.Helpers
{
    public static class HttpResponse
    {
        public static ContentResult Response(string content, int? statusCode, string contentType)
        {
            return new ContentResult()
            {
                Content = content,
                StatusCode = statusCode,
                ContentType = contentType
            };
        }

        public static ContentResult Response(string content, HttpStatusCode statusCode, string contentType = "application/json; charset=utf-8")
        {
            return Response(content, statusCode.GetHashCode(), contentType);
        }

        public static async Task<ContentResult> Response(HttpResponseMessage http)
        {
            var contentType = http.Content.Headers.ContentType?.ToString();
            var content = await http.Content.ReadAsStringAsync();
            var statusCode = http.StatusCode.GetHashCode();

            return Response(content, statusCode.GetHashCode(), contentType);
        }
    }
}
