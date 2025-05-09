using System.Reflection;

using Microsoft.OpenApi.Models;

using MirthSystems.Pulse.Infrastructure.Extensions;

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
        builder.Services.AddAuthentication().AddJwtBearer();
        builder.Services.AddAuthorization();

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

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles();
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAllOrigins");

        app.UseHttpsRedirection();

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}