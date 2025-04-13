namespace Pulse.Api
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    using NodaTime;
    using Pulse.Api.Extensions;
    using Pulse.Infrastructure.Extensions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            var azureMapsSubscriptionKey = builder.Configuration["AzureMaps:SubscriptionKey"];
            if (string.IsNullOrEmpty(azureMapsSubscriptionKey))
            {
                throw new InvalidOperationException("Azure Maps subscription key is not configured.");
            }

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PulseWebClientPolicy", policy =>
                {
                    policy.WithOrigins(
                            "https://localhost:7254",
                            "http://localhost:5002")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            builder.Services.AddPulseInfrastructure(connectionString, azureMapsSubscriptionKey);
            builder.Services.AddSingleton<IClock>(SystemClock.Instance);

            builder.Services.AddAuthentication().AddJwtBearer();
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerService();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseStaticFiles();
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("PulseWebClientPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}