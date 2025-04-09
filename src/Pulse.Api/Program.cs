namespace Pulse.Api
{ 
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

            builder.Services.AddPulseInfrastructure(connectionString);
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

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}