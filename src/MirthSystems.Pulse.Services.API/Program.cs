using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using MirthSystems.Pulse.Core.Interfaces;
using MirthSystems.Pulse.Infrastructure.Data;
using MirthSystems.Pulse.Infrastructure.Data.Repositories;
using MirthSystems.Pulse.Infrastructure.Extensions;
using MirthSystems.Pulse.Infrastructure.Services;
using MirthSystems.Pulse.Services.API.Test.Interfaces;
using MirthSystems.Pulse.Services.API.Test.Models;
using MirthSystems.Pulse.Services.API.Test.Services;

using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

using Serilog;

using System.Security.Claims;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddSerilog(
                logger: Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(builder.Configuration.GetSection("Logging:Serilog"))
                            .CreateBootstrapLogger(),
                dispose: true
            );

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        builder.Services.AddServiceDefaults();
        builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("PostgresDbConnection"));
        builder.Services.AddAzureMaps(builder.Configuration.GetValue<string>("AzureMaps:SubscriptionKey"));

        builder.Services.AddScoped<IMessageService, MessageService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Auth0:Authority"];
                options.Audience = builder.Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        builder.Services.AddOpenApiDocument(document =>
        {
            document.Title = "Pulse API";
            document.Version = "v1";
            document.DocumentName = "v1";
            document.Description = "API for the Pulse nightlife discovery platform";

            document.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}