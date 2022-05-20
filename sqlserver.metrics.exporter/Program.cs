using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SqlServer.metrics.exporter
{
    public class Program
    {
        private const string APPSETTINGS_PATH = "./config/appsettings.json";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    }).ConfigureAppConfiguration(b =>
                    {
                        b.AddJsonFile(APPSETTINGS_PATH, true, false);
                    });
        }
    }
}
