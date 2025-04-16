namespace Pulse.Api
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
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
                options.AddPolicy("PulseClientPolicy", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000")
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

            // Replace Swagger with NSwag
            var nswagVersion = "v1";
            builder.Services.AddNSwagService(nswagVersion);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseStaticFiles();

                app.UseOpenApi(options => {
                    options.Path = "/swagger/{documentName}/swagger.json";
                });

                app.UseSwaggerUi(options => {
                    options.Path = "/swagger";
                    options.DocumentPath = $"/swagger/{nswagVersion}/swagger.json"; // Match your DocumentName
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("PulseClientPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}