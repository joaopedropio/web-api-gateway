using Newtonsoft.Json;

namespace WebAPIGateway
{
    public class ServiceUrl
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }
}
