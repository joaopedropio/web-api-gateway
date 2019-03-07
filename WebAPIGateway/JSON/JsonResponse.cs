using System;
using System.Net;

namespace WebAPIGateway.JSON
{
    public class JsonResponse : IJsonResponse
    {
        public IJsonData Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public JsonResponse() { }

        public JsonResponse(IJsonData data, HttpStatusCode statusCode)
        {
            this.Data = data;
            this.StatusCode = statusCode;
        }

        public override bool Equals(object obj)
        {
            JsonResponse json;
            try
            {
                json = (JsonResponse)obj;
            }
            catch (Exception)
            {
                return false;
            }
            
            return this.Data == json.Data && this.StatusCode == json.StatusCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data, StatusCode);
        }
    }
}
