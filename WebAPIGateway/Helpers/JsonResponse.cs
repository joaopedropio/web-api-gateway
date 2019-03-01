using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPIGateway.Helpers
{
    public class JsonResponse
    {
        public object Data;
        public HttpStatusCode StatusCode;

        public JsonResponse()
        { }

        public JsonResponse(object data, HttpStatusCode statusCode)
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
