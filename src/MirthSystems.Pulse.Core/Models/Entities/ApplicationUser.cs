namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a user authenticated via Auth0 in the Pulse platform.
    /// </summary>
    /// <remarks>
    /// Central entity for authorization, supporting both application-wide and venue-specific roles.
    /// Example: A user with UserObjectId "auth0|12345" named "Jane Doe" with email "jane@example.com".
    /// </remarks>
    public class ApplicationUser
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier from Auth0.
        /// </summary>
        /// <remarks>
        /// Example: "auth0|12345" for a user authenticated via Auth0.
        /// </remarks>
        public required string UserObjectId { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        /// <remarks>
        /// Example: "jane@example.com" for contact purposes.
        /// </remarks>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets when the user was created.
        /// </summary>
        /// <remarks>
        /// Example: "2023-01-15T10:00:00Z" for a user registered on that date.
        /// </remarks>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets whether the user account is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active user, false for a suspended account.
        /// </remarks>
        public bool IsActive { get; set; } = true;

        public virtual ICollection<ApplicationUserRole> Roles { get; set; } = [];
        public virtual ICollection<ApplicationUserPermission> Permissions { get; set; } = [];
        public virtual ICollection<VenueUser> Venues { get; set; } = [];
        public virtual ICollection<Special> CreatedSpecials { get; set; } = [];
        public virtual ICollection<Special> UpdatedSpecials { get; set; } = [];
    }
}
