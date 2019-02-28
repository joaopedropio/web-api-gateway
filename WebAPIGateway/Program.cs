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
            Configuration.Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(Configuration.URL)
                .Build();
        }
    }
}
