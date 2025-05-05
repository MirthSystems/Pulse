namespace MirthSystems.Pulse.Services.DatabaseMigrations
{
    using MirthSystems.Pulse.Infrastructure.Data;
    using System;

    public static class DataSeeder
    {
        public static async Task RunAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            return; // No seeding logic implemented yet
        }
    }
}
