using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebAPIGateway.Domain
{
    public partial class Service : IService
    {
        public static IService ParseService(string service)
        {
            try
            {
                var arr = service.Split(',');

                if (arr.Length != 2)
                    throw new Exception();

                return new Service(arr[0], arr[1]);
            }
            catch (Exception ex)
            {
                throw new Exception("Service provided is invalid", ex);
            }
        }

        public static IService ParseService(Stream body)
        {
            var content = new StreamReader(body).ReadToEnd();
            return JsonConvert.DeserializeObject<Service>(content);
        }

        public static IList<IService> ParseServices(string services)
        {
            if (string.IsNullOrEmpty(services))
                return new List<IService>();

            return services.Split(';').Select(s => ParseService(s)).ToList();
        }
    }
}
