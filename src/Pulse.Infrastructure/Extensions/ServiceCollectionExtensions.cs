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
    using Pulse.Core.Models.Options;
    using Pulse.Infrastructure.Repositories;
    using Pulse.Infrastructure.Services;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPulseInfrastructure(
            this IServiceCollection services,
            string postgresConnectionString,
            string azureMapsSubscriptionKey)
        {
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new ArgumentException("Connection string is required", nameof(postgresConnectionString));
            }

            if (string.IsNullOrEmpty(azureMapsSubscriptionKey))
            {
                throw new ArgumentException("Azure Maps subscription key is required", nameof(azureMapsSubscriptionKey));
            }

            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));

            services.AddScoped<IVenueLocationService, VenueLocationService>();

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
                        options.EnableSensitiveDataLogging(true);
                    }
                    else
                    {
                        options.EnableSensitiveDataLogging(false);
                    }
                }
            });

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

            return services;
        }
    }
}
