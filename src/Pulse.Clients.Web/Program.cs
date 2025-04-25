namespace Pulse.Clients.Web
{
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.FluentUI.AspNetCore.Components;
    using Microsoft.Graph;

    using Pulse.Clients.Web.Extensions;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var baseUrl = string.Join("/",
                builder.Configuration.GetSection("MicrosoftGraph")["BaseUrl"] ??
                    "https://graph.microsoft.com",
                builder.Configuration.GetSection("MicrosoftGraph")["Version"] ??
                    "v1.0");

            var scopes = builder.Configuration.GetSection("MicrosoftGraph:Scopes")
                .Get<List<string>>() ?? ["user.read"];

            builder.Services.AddGraphClient(baseUrl, scopes);
            builder.Services.AddFluentUIComponents();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
            });

            await builder.Build().RunAsync();
        }
    }
}
