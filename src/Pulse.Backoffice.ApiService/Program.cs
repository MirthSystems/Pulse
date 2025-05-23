
using Microsoft.Extensions.Configuration;

using Pulse.Core.Configurations;
using Pulse.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var appConfigSection = builder.Configuration.GetSection("Pulse");
        if (appConfigSection is null)
        {
            throw new ArgumentNullException(nameof(appConfigSection));
        }

        var appConfig = appConfigSection.Get<ApplicationConfiguration>();

        builder.Services.Configure<ApplicationConfiguration>(appConfigSection);   

        builder.Services.AddApplicationServices(appConfig);

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

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}