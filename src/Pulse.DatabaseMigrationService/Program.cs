namespace Pulse.DatabaseMigrationService
{
    using Pulse.Infrastructure;
    using Pulse.Infrastructure.Extensions;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            builder.Services.AddScoped<DatabaseSeeder>();
            builder.Services.AddPulseInfrastructure(connectionString);

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}