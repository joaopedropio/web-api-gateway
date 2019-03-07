using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPIGateway.JSON
{
    public interface IJsonResponse
    {
        IJsonData Data { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}
