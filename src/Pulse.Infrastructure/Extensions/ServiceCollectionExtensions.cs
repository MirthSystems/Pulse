namespace Pulse.Infrastructure.Extensions
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using NodaTime;
    using NodaTime.TimeZones;

    using Pulse.Core.Contracts;
    using Pulse.Infrastructure.Repositories;
    using Pulse.Infrastructure.Services;

    /// <summary>
    /// Extension methods for registering Pulse infrastructure services with DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all Pulse infrastructure services to the DI container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="postgresConnectionString">PostgreSQL connection string</param>
        /// <param name="azureMapsSubscriptionKey">Azure Maps subscription key</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddPulseInfrastructure(
            this IServiceCollection services,
            string postgresConnectionString,
            string azureMapsSubscriptionKey)
        {
            if (string.IsNullOrEmpty(azureMapsSubscriptionKey))
            {
                throw new ArgumentException("Azure Maps subscription key is required", nameof(azureMapsSubscriptionKey));
            }

            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));

            services.AddPulseDatabase(postgresConnectionString);

            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<IVenueTypeRepository, VenueTypeRepository>();
            services.AddScoped<ISpecialRepository, SpecialRepository>();
            services.AddScoped<IOperatingScheduleRepository, OperatingScheduleRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagToSpecialLinkRepository, TagToSpecialLinkRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILocationService>(sp => new LocationService(
                azureMapsSubscriptionKey,
                sp.GetRequiredService<IClock>(),
                sp.GetRequiredService<IDateTimeZoneProvider>(),
                sp.GetRequiredService<ILogger<LocationService>>()));

            services.AddScoped<IVenueLocationService, VenueLocationService>();

            return services;
        }

        /// <summary>
        /// Adds Pulse database context and repository services to the DI container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="postgresConnectionString">PostgreSQL connection string</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddPulseDatabase(
            this IServiceCollection services,
            string postgresConnectionString)
        {
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new ArgumentException("Connection string is required", nameof(postgresConnectionString));
            }

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

                options.UseNpgsql(postgresConnectionString, npg =>
                {
                    npg.UseNodaTime();
                    npg.UseNetTopologySuite();
                    npg.EnableRetryOnFailure(3);
                    npg.MaxBatchSize(100);
                })
                .UseSnakeCaseNamingConvention();

                if (loggerFactory != null)
                {
                    options.UseLoggerFactory(loggerFactory);
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        options.EnableSensitiveDataLogging();
                    }
                }
            });

            return services;
        }
    }
}
