namespace Pulse.Clients.Web
{
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.FluentUI.AspNetCore.Components;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddFluentUIComponents();

            builder.Services.AddAuth0Authentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
