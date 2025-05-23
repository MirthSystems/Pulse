namespace Pulse.Extensions
{
    using Azure;
    using Azure.Maps.Geolocation;
    using Azure.Maps.Search;
    using Azure.Maps.TimeZones;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NodaTime;

    using Pulse.Core.Configurations;
    using Pulse.Data.Context;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ApplicationConfiguration? configuration = null)
        {         
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.ConnectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.UseNodaTime();
                    npgsqlOptions.UseNetTopologySuite();
                })
                .UseSnakeCaseNamingConvention();

                options.EnableSensitiveDataLogging();
            });

            var azureKeyCredentials = new AzureKeyCredential(configuration.AzureMaps.SubscriptionKey);
            services.AddScoped(_ => new MapsGeolocationClient(azureKeyCredentials));
            services.AddScoped(_ => new MapsSearchClient(azureKeyCredentials));
            services.AddScoped(_ => new MapsTimeZoneClient(azureKeyCredentials));

            return services;
        }
    }
}
