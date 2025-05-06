namespace MirthSystems.Pulse.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    using MirthSystems.Pulse.Infrastructure.Data;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Events;
    using System;
    using Azure;
    using Azure.Maps.Search;
    using Azure.Maps.TimeZones;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Infrastructure.Services;
    using MirthSystems.Pulse.Infrastructure.Data.Repositories;
    using NodaTime;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for configuring services for the Pulse application.
    /// </summary>
    /// <remarks>
    /// <para>This class provides methods to register and configure essential services for the application, including:</para>
    /// <para>- Application default services such as time providers and service interfaces</para>
    /// <para>- Database context and related repositories</para>
    /// <para>- Azure Maps integration</para>
    /// <para>- Health checks for monitoring application components</para>
    /// </remarks>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds default application services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for method chaining.</returns>
        /// <remarks>
        /// <para>This method registers the following services:</para>
        /// <para>- System clock as a singleton for consistent time operations</para>
        /// <para>- Core service interfaces with their implementations</para>
        /// </remarks>
        public static IServiceCollection AddServiceDefaults(this IServiceCollection services)
        {
            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IOperatingScheduleService, OperatingScheduleService>();
            services.AddScoped<ISpecialService, SpecialService>();

            return services;
        }

        /// <summary>
        /// Adds and configures the application database context and related services.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="postgresConnectionString">The PostgreSQL connection string.</param>
        /// <returns>The service collection for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if the connection string is null or empty.</exception>
        /// <remarks>
        /// <para>This method configures the following database-related services:</para>
        /// <para>- Entity Framework Core with PostgreSQL provider</para>
        /// <para>- NodaTime for date and time handling</para>
        /// <para>- NetTopologySuite for spatial data</para>
        /// <para>- Unit of Work pattern implementation for transaction management</para>
        /// <para>- Database health check for monitoring database connectivity</para>
        /// </remarks>
        public static IServiceCollection AddApplicationDbContext(
            this IServiceCollection services,
            string? postgresConnectionString = null)
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
                    npg.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npg.UseNodaTime();
                    npg.UseNetTopologySuite();
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

                        return services;
        }

        /// <summary>
        /// Adds and configures the Azure Maps services for geospatial functionality.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="azureMapsSubscriptionKey">The Azure Maps subscription key.</param>
        /// <returns>The service collection for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if the Azure Maps subscription key is null or empty.</exception>
        /// <remarks>
        /// <para>This method configures Azure Maps integration for the following capabilities:</para>
        /// <para>- Geocoding and reverse geocoding of addresses</para>
        /// <para>- Search for locations and places of interest</para>
        /// <para>- Time zone determination for locations</para>
        /// <para>- External health check for monitoring Azure Maps API availability</para>
        /// </remarks>
        public static IServiceCollection AddAzureMaps(
            this IServiceCollection services,
            string? azureMapsSubscriptionKey = null)
        {
            if (string.IsNullOrEmpty(azureMapsSubscriptionKey))
            {
                throw new ArgumentException("Azure Subscription key is required.", nameof(azureMapsSubscriptionKey));
            }

            var azureMapsKeyCredential = new AzureKeyCredential(azureMapsSubscriptionKey);

            services.AddSingleton<IAzureMapsApiService>(serviceProvider =>
                new AzureMapsApiService(azureMapsKeyCredential));

                        return services;
        }
    }
}
