using System;
using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.Common.Extensions;
using AudiioScraper.Worker.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AudiioScraper.Worker.Workers
{
    public class DownloaderWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DownloaderWorker> _logger;

        public DownloaderWorker(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<DownloaderWorker> logger)
        {
            _configuration = configuration;
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
                var downloaderHelper = serviceScope.ServiceProvider.GetService<DownloaderHelper>();

                if (downloaderHelper != null)
                {
                    await downloaderHelper.Go(stoppingToken);
                }
                else
                {
                    _logger.LogCritical("DOWNLOADER-HELPER COULD NOT BE CONSTRUCTED!!!");
                }

                await Task.Delay(TimeSpan.FromSeconds(_configuration.GetDownloaderIntervalInSeconds()), stoppingToken);
            }
        }
    }
}