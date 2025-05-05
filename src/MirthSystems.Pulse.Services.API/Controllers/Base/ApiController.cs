namespace MirthSystems.Pulse.Services.API.Controllers.Base
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Models.Responses;

    [ApiController]
    [Produces("application/json")]
    public abstract class ApiController : ControllerBase
    {
        /// <summary>
        /// Gets the current user's ID from claims
        /// </summary>
        protected string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
