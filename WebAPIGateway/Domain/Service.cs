using Newtonsoft.Json;
using System;
using WebAPIGateway.JSON;

namespace WebAPIGateway.Domain
{
    public partial class Service : IService
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
