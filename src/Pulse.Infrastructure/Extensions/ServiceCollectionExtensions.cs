namespace Pulse.Infrastructure.Extensions
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPulseInfrastructure(this IServiceCollection services, string postgresConnectionString)
        {
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(postgresConnectionString, npg =>
                {
                    npg.UseNodaTime();
                    npg.UseNetTopologySuite();
                }).UseSnakeCaseNamingConvention();
            });
            return services;
        }
    }
}
