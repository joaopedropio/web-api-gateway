using Microsoft.Extensions.Configuration;

namespace WebAPIGateway
{
    public class Configuration
    {
        public string URL { get; private set; }
        public string DBServer { get; private set; }
        public IConfigurationSection Logging { get; set; }
        public string Services { get; set; }

        public Configuration() : this(new ConfigurationBuilder().AddEnvironmentVariables().Build()) { }

        public Configuration(IConfigurationRoot configuration)
        {
            var domain = configuration.GetValue<string>("API_DOMAIN") ?? "*";
            var port = configuration.GetValue<string>("API_PORT") ?? "80";
            URL = string.Format($"http://{domain}:{port}");
            DBServer = configuration.GetValue<string>("DB_SERVER") ?? "localhost";
            Logging = configuration.GetSection("Logging");
            Services = configuration.GetValue<string>("SERVICES");
        }
    }
}
