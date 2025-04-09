namespace Pulse.Infrastructure.Extensions
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Pulse.Core.Contracts;
    using Pulse.Infrastructure.Services;

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

            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<ISpecialService, SpecialService>();
            services.AddScoped<ILocationService, LocationService>();

            return services;
        }
    }
}
