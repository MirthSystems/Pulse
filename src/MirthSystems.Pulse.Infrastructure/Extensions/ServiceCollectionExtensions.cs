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

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLogging(
            this IServiceCollection services,
            IConfigurationSection? serilogConfigurationSection = null)
        {
            try
            {
                if (serilogConfigurationSection == null)
                {
                    ConfigureDefaultLogger();
                }
                else
                {
                    ConfigureLogger();
                }
            }
            catch (Exception ex)
            {
                ConfigureDefaultLogger();

                Log.Logger.Warning("Failed to configure Serilog from configuration section: {ErrorMessage}", ex.Message);
                Log.Logger.Information("Falling back to default logging configuration");
            }

            return services;

            void ConfigureLogger()
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(
                        logger: 
                            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(serilogConfigurationSection)
                                .CreateLogger(), 
                        dispose: true
                    );
                });
            }

            void ConfigureDefaultLogger()
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(
                        logger:
                            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Information()
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                .MinimumLevel.Override("System", LogEventLevel.Warning)
                                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                                .Enrich.FromLogContext()
                                .Enrich.WithMachineName()
                                .Enrich.WithThreadId()
                                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                                .WriteTo.File(
                                    path: "logs/mirthsystems-pulse-.log",
                                    rollingInterval: RollingInterval.Day,
                                    retainedFileCountLimit: 7,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}")
                                .CreateLogger(), 
                        dispose: true
                    );
                });
            }
        }

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
    }
}
