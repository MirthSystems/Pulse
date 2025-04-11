namespace Pulse.Infrastructure.Extensions
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Infrastructure.Repositories;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPulseInfrastructure(
            this IServiceCollection services,
            string postgresConnectionString)
        {
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            services.AddSingleton<IClock>(SystemClock.Instance);

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

            return services;
        }
    }
}
