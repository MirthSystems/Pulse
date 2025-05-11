using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using MirthSystems.Pulse.Infrastructure.Extensions;
using MirthSystems.Pulse.Services.API.Authorization;
using MirthSystems.Pulse.Services.API.Extensions;

using Serilog;

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

        builder.Services.AddServiceDefaults();
        builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("PostgresDbConnection"));
        builder.Services.AddAzureMaps(builder.Configuration.GetSection("AzureMaps")["SubscriptionKey"]);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(
                    builder.Configuration.GetValue<string>("Authentication:ClientOriginUrl")!)
                    .WithHeaders(new string[] {
                        HeaderNames.ContentType,
                        HeaderNames.Authorization,
                    })
                    .WithMethods("GET")
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
            });
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var audience =
                      builder.Configuration.GetValue<string>("Authentication:Audience");

                options.Authority =
                      $"https://{builder.Configuration.GetValue<string>("Authentication:Domain")}/";
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("System.Administrator", policy =>
            {
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue:create"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue:update"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue:delete"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue:update_schedule"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue_special:create"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue_special:update"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue_special:delete"));
            });
            options.AddPolicy("Content.Manager", policy =>
            {
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue:update"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue:update_schedule"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue_special:create"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue_special:update"));
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("venue_special:delete"));
            });
            options.AddPolicy("Test.User", policy =>
            {
                policy.Requirements.Add(new RoleBasedAccessControlRequirement("test:read-message"));
            });
        });


        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Pulse API",
                Description = "Learn how to protect your .NET applications with Auth0",
                Contact = new OpenApiContact
                {
                    Name = ".NET Identity with Auth0",
                    Url = new Uri("https://a0.to/dotnet-templates/webapi")
                },
                Version = "v1.0.0"
            });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securitySchema);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { "Bearer" } }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles();
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseErrorHandler();
        app.UseSecureHeaders();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}