using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AudiioScraper.Worker.Workers
{
    public class StartupWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<StartupWorker> _logger;

        public StartupWorker(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<StartupWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<AudiioScraperDbContext>();

            if (dbContext != null)
            {
                await dbContext.Database.MigrateAsync(stoppingToken);
            }
            else
            {
                _logger.LogCritical("DATABASE-CONTEXT COULD NOT BE CONSTRUCTED!!!");
            }
        }
    }
}