using AudiioScraper.DataAccess;
using AudiioScraper.Worker.Helpers;
using AudiioScraper.Worker.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AudiioScraper.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AudiioScraperDbContext>();
                    services.AddTransient<ScraperHelper>();
                    services.AddTransient<DownloaderHelper>();
                    services.AddHostedService<StartupWorker>();
                    services.AddHostedService<ScraperWorker>();
                    services.AddHostedService<DownloaderWorker>();
                });
    }
}