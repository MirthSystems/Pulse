namespace Pulse.Clients.Web
{
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.FluentUI.AspNetCore.Components;
    using Microsoft.Graph;

    using Pulse.Clients.Web.Extensions;
    using Pulse.Clients.Web.Factories;
    using Pulse.Clients.Web.Services;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var graphConfigSection = builder.Configuration.GetSection("MicrosoftGraph");
            var baseUrl = string.Join("/",
                graphConfigSection["BaseUrl"] ?? "https://graph.microsoft.com",
                graphConfigSection["Version"] ?? "v1.0");

            var scopes = builder.Configuration.GetSection("MicrosoftGraph:Scopes")
                .Get<List<string>>() ?? ["user.read"];

            builder.Services.AddGraphClient(baseUrl, scopes);

            builder.Services.AddHttpClient<PulseApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });

            builder.Services.AddHttpClient<WeatherApiService>(client =>
            {
                // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
                // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
                client.BaseAddress = new("https+http://apiservice");
            });

            builder.Services.AddFluentUIComponents();

            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, RemoteUserAccount>(options =>
            {
                builder.Configuration.Bind("AzureAd",
                options.ProviderOptions.Authentication);
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, AccountFactory>();

            await builder.Build().RunAsync();
        }
    }
}
