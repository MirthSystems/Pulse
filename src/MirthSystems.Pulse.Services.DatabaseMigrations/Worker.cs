namespace MirthSystems.Pulse.Services.DatabaseMigrations
{
    using System.ComponentModel;
    using System.Diagnostics;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;

    using MirthSystems.Pulse.Infrastructure.Data;

    public class Worker(
        IServiceProvider serviceProvider,
        IHostEnvironment hostEnvironment,
        IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        private readonly ActivitySource _activitySource = new(hostEnvironment.ApplicationName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity(hostEnvironment.ApplicationName, ActivityKind.Client);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await EnsureDatabaseAsync(dbContext, cancellationToken);
                await RunMigrationAsync(dbContext, cancellationToken);
            }
            catch (Exception ex)
            {
                activity?.AddException(ex);
                throw;
            }

            hostApplicationLifetime.StopApplication();
        }

        private static async Task EnsureDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                if (!await dbCreator.ExistsAsync(cancellationToken))
                {
                    await dbCreator.CreateAsync(cancellationToken);
                }
            });
        }

        private static async Task RunMigrationAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                await dbContext.Database.MigrateAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });
        }
    }
}