using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebAPIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configuration.Build();
            BuildWebHost().Build().Run();
        }

        public static IWebHostBuilder BuildWebHost()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseUrls(Configuration.URL);
        }
    }
}
