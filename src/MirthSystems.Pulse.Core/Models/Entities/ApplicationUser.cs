namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a user of the application who has authenticated through the external identity provider.
    /// </summary>
    /// <remarks>
    /// <para>Users are authenticated via Auth0 but authorization is managed within the application.</para>
    /// <para>Each user can have application-wide roles and permissions, as well as venue-specific roles.</para>
    /// </remarks>
    public class ApplicationUser
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user within the application.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the external identity provider's unique identifier for the user.
        /// </summary>
        /// <remarks>
        /// <para>This is the Auth0 user ID, typically formatted as "auth0|12345abcde".</para>
        /// <para>It is used to match authenticated users with their application user record.</para>
        /// </remarks>
        public required string UserObjectId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this user record was created in the system.
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        /// <remarks>
        /// <para>Inactive users cannot log in even if their Auth0 credentials are valid.</para>
        /// <para>This provides an application-level mechanism to disable access.</para>
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of application roles assigned to this user.
        /// </summary>
        public virtual ICollection<ApplicationUserRole> Roles { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of permissions directly assigned to this user.
        /// </summary>
        public virtual ICollection<ApplicationUserPermission> Permissions { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of venue-user associations for this user.
        /// </summary>
        /// <remarks>
        /// <para>This represents all venues that this user is associated with.</para>
        /// <para>The VenueUser entity contains the roles the user has for each venue.</para>
        /// </remarks>
        public virtual ICollection<VenueUser> VenueUsers { get; set; } = [];
    }
}
