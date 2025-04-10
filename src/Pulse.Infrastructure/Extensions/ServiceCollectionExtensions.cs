namespace Pulse.Infrastructure.Extensions
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Infrastructure.Repositories;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPulseInfrastructure(this IServiceCollection services, string postgresConnectionString)
        {
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(postgresConnectionString, npg =>
                {
                    npg.UseNodaTime();
                    npg.UseNetTopologySuite();
                }).UseSnakeCaseNamingConvention();
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
