namespace Pulse.Clients.Web
{
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.FluentUI.AspNetCore.Components;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var apiBaseUrl = builder.Configuration["PulseApi:BaseUrl"] ?? "https://localhost:7253";
            var apiScopes = builder.Configuration.GetSection("PulseApi:Scopes").Get<string[]>()
                ?? new[] { "openid", "profile", "email" };

            builder.Services.AddHttpClient("PulseApi", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler(sp =>
            {
                var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(
                        authorizedUrls: new[] { apiBaseUrl },
                        scopes: apiScopes);
                return handler;
            });

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("PulseApi"));

            builder.Services.AddFluentUIComponents();

            builder.Services.AddAuth0Authentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
