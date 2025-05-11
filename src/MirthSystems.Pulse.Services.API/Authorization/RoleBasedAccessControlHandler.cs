namespace MirthSystems.Pulse.Services.API.Authorization
{
    using Microsoft.AspNetCore.Authorization;

    public class RoleBasedAccessControlHandler : AuthorizationHandler<RoleBasedAccessControlRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleBasedAccessControlRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "permissions"))
            {
                return Task.CompletedTask;
            }

            var permission = context.User.FindFirst(c => c.Type == "permissions" && c.Value == requirement.Permission);

            if (permission == null)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
