namespace MirthSystems.Pulse.Services.DatabaseMigrations
{

    using MirthSystems.Pulse.Infrastructure.Data;
    using Npgsql.EntityFrameworkCore.PostgreSQL;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var serilogConfigurationSection = builder.Configuration.GetSection("Serilog");
            var dbConnectionString = builder.Configuration.GetConnectionString("PostgresDbConnection");

            builder.Services.AddApplicationLogging(serilogConfigurationSection);
            builder.Services.AddApplicationDbContext(dbConnectionString);
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}