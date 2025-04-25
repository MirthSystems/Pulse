namespace Pulse.Clients.Web.Factories
{
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.Graph;
    using Microsoft.Kiota.Abstractions.Authentication;
    using System.Security.Claims;

    public class AccountFactory(IAccessTokenProviderAccessor accessor,
            IServiceProvider serviceProvider, ILogger<AccountFactory> logger,
            IConfiguration config)
        : AccountClaimsPrincipalFactory<RemoteUserAccount>(accessor)
    {
        private readonly ILogger<AccountFactory> logger = logger;
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private readonly string? baseUrl = string.Join("/",
            config.GetSection("MicrosoftGraph")["BaseUrl"] ??
                "https://graph.microsoft.com",
            config.GetSection("MicrosoftGraph")["Version"] ??
                "v1.0");

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity is not null &&
                initialUser.Identity.IsAuthenticated)
            {
                var userIdentity = initialUser.Identity as ClaimsIdentity;

                if (userIdentity is not null && !string.IsNullOrEmpty(baseUrl))
                {
                    try
                    {
                        var client = new GraphServiceClient(
                            new HttpClient(),
                            serviceProvider
                                .GetRequiredService<IAuthenticationProvider>(),
                            baseUrl);

                        var user = await client.Me.GetAsync();

                        if (user is not null)
                        {
                            userIdentity.AddClaim(new Claim("mobilephone",
                                user.MobilePhone ?? "(000) 000-0000"));
                            userIdentity.AddClaim(new Claim("officelocation",
                                user.OfficeLocation ?? "Not set"));
                        }
                    }
                    catch (AccessTokenNotAvailableException exception)
                    {
                        exception.Redirect();
                    }
                }
            }

            return initialUser;
        }
    }
}
