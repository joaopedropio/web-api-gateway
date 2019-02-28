using System.Collections.Generic;
using System.Linq;

namespace WebAPIGateway
{
    public class Service
    {
        public string Name { get; set; }
        public string URL { get; set; }

        public Service(string name, string url)
        {
            Name = name;
            URL = url;
        }

        public Service(string service)
        {
            var arr = service.Split(',');
            this.Name = arr[0];
            this.URL = arr[1];
        }

        public static IEnumerable<Service> ParseServices(string services)
        {
            return services.Split(';').Select(s => new Service(s));
        }

        public override bool Equals(object obj)
        {
            var service = (Service)obj;
            return this.Name == service.Name && this.URL == service.URL;
        }
    }
}
