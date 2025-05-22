namespace Pulse.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Pulse.Core.Configurations;
    using Pulse.Data.Context;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ApplicationConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.PostgresConnectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.UseNodaTime();
                    npgsqlOptions.UseNetTopologySuite();
                })
                .UseSnakeCaseNamingConvention();

                options.EnableSensitiveDataLogging();
            });

            // Add additional services (e.g., repositories, application services) here

            return services;
        }
    }
}
