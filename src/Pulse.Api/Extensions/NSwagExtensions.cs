namespace Pulse.Api.Extensions
{
    using NSwag.Generation.Processors.Security;
    using NSwag;

    public static class NSwagExtensions
    {
        public static IServiceCollection AddNSwagService(this IServiceCollection services, string version = null!)
        {
            // Register NSwag services
            services.AddOpenApiDocument(options =>
            {
                // Document details
                options.DocumentName = version;
                options.Title = $"Pulse API {version}";
                options.ApiGroupNames = new[] { version };
                options.Description = $"Pulse Backend API {version}";
                options.Version = version;

                options.SchemaSettings.SchemaType = NJsonSchema.SchemaType.OpenApi3;

                // Contact information
                options.PostProcess = document =>
                {
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = ".NET Identity with Auth0",
                        Url = "https://a0.to/dotnet-templates/webapi"
                    };
                };

                // Add JWT bearer authentication
                options.AddSecurity("JWT", Enumerable.Empty<string>(),
                    new NSwag.OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Using the Authorization header with the Bearer scheme."
                    });

                options.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            return services;
        }
    }
}