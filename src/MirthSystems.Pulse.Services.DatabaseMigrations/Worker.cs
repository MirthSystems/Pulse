namespace MirthSystems.Pulse.Services.DatabaseMigrations
{
    using System.ComponentModel;
    using System.Diagnostics;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;

    using MirthSystems.Pulse.Infrastructure.Data;

    using Serilog;

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
            using (var scope = this._serviceProvider.CreateScope())
            {
                this._logger.LogInformation("Starting database migration check...");

                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await dbContext.Database.MigrateAsync(stoppingToken);
                    this._logger.LogInformation("Database migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "Migration failed.");
                    throw;
                }
            }
        }
    }
}
