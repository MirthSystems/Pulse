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

            builder.AddServiceDefaults();            

            builder.Services.AddApplicationLogging(builder.Configuration.GetSection("Serilog"));
            builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("PostgresDbConnection"));

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}