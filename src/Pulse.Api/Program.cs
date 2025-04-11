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

            builder.Services.AddPulseInfrastructure(connectionString, azureMapsSubscriptionKey);
            builder.Services.AddSingleton<IClock>(SystemClock.Instance);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
                    options.Audience = builder.Configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("VenueManagement", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == "permissions" && c.Value == "write:venues") ||
                            (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" &&
                             (c.Value == "System Administrator" || c.Value == "Venue Manager"))
                        )
                    ));

                options.AddPolicy("SpecialManagement", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == "permissions" && c.Value == "write:specials") ||
                            (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" &&
                             (c.Value == "System Administrator" || c.Value == "Venue Manager"))
                        )
                    ));
            });

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

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}