namespace MirthSystems.Pulse.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

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

    public static class ServiceCollectionExtensions
    {
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

            return services;
        }

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
