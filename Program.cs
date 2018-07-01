using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebAPIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new Configuration();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(config.URL)
                .Build();
        }
    }
}
