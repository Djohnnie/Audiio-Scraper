using System;
using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.Worker.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AudiioScraper.Worker.Workers
{
    public class ScraperWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ScraperWorker> _logger;

        public ScraperWorker(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ScraperWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.Now);

                using var serviceScope = _serviceScopeFactory.CreateScope();
                var scraperHelper = serviceScope.ServiceProvider.GetService<ScraperHelper>();

                if (scraperHelper != null)
                {
                    await scraperHelper.Go(stoppingToken);
                }
                else
                {
                    _logger.LogCritical("SCRAPER-HELPER COULD NOT BE CONSTRUCTED!!!");
                }

                await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
            }
        }
    }
}