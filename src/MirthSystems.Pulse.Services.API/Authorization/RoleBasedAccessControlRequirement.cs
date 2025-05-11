namespace MirthSystems.Pulse.Services.API.Authorization
{
    using Microsoft.AspNetCore.Authorization;

    public class RoleBasedAccessControlRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public RoleBasedAccessControlRequirement(string permission)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}
