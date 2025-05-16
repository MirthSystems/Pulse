namespace MirthSystems.Pulse.Services.DatabaseMigrations
{

    using MirthSystems.Pulse.Infrastructure.Data;
    using Npgsql.EntityFrameworkCore.PostgreSQL;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Infrastructure.Extensions;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddSerilog(
                    logger: Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(builder.Configuration.GetSection("Logging:Serilog"))
                                .CreateBootstrapLogger(),
                    dispose: true
                );

            builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("PostgresDbConnection"));

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}