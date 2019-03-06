using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebAPIGateway.Domain
{
    public class Service : IService
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

        [JsonConstructor]
        public Service(string name, string url)
        {
            Name = name;
            URL = url;
        }

        public Service(string service)
        {
            try
            {
                var arr = service.Split(',');

                if (arr.Length != 2)
                    throw new Exception();

                this.Name = arr[0];
                this.URL = arr[1];
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

        public static IList<Service> ParseServices(string services)
        {
            if (string.IsNullOrEmpty(services))
                return new List<Service>();

            return services.Split(';').Select(s => new Service(s)).ToList();
        }

        public override bool Equals(object obj)
        {
            var service = (Service)obj;
            return this.Name == service.Name && this.URL == service.URL;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, URL);
        }
    }
}
