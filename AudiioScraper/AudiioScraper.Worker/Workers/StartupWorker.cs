using AudiioScraper.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(stoppingToken);

                if (pendingMigrations.Any())
                {
                    _logger.LogInformation($"There are {pendingMigrations.Count()} database migrations.");

                    await dbContext.Database.MigrateAsync(stoppingToken);
                    _logger.LogInformation("Database has been successfully migrated.");
                }
                else
                {
                    _logger.LogInformation("Database is up-to-date.");
                }
            }
            else
            {
                _logger.LogCritical("DATABASE-CONTEXT COULD NOT BE CONSTRUCTED!!!");
            }
        }
    }
}