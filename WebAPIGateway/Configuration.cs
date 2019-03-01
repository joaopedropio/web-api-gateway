using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebAPIGateway
{
    public static class Configuration
    {
        public static IConfigurationRoot configuration;
        public static string URL
        {
            get
            {
                var domain = configuration.GetValue<string>("API_DOMAIN") ?? "*";
                var port = configuration.GetValue<string>("API_PORT") ?? "80";
                return string.Format($"http://{domain}:{port}");
            }
        }
        public static string CacheDomain => configuration.GetValue<string>("CACHE_DOMAIN") ?? "localhost";
        public static IConfigurationSection Logging => configuration.GetSection("Logging");
        public static IList<Service> Services => Service.ParseServices(configuration.GetValue<string>("SERVICES"));

        public static bool UseRedisCache => configuration.GetValue<bool>("USE_REDIS");

        public static void Build(IConfigurationRoot configurationRoot)
        {
            configuration = configurationRoot;
        }

        public static void Build()
        {
            configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        }
    }
}
