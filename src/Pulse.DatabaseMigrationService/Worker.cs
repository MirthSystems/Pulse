namespace Pulse.DatabaseMigrationService
{
    using Microsoft.EntityFrameworkCore;

    using Pulse.Infrastructure;

    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._logger.LogInformation("Starting database migration process.");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await dbContext.Database.MigrateAsync(stoppingToken);

                    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
                    await seeder.SeedAsync();
                }

                this._logger.LogInformation("Database migration and seeding completed successfully.");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }
    }
}
