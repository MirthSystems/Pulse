namespace MirthSystems.Pulse.Core.Models.Entities
{
    using System.Security.Claims;
    using NodaTime;

    /// <summary>
    /// Represents a system-wide role within the application's authorization system.
    /// </summary>
    /// <remarks>
    /// Application roles apply globally and group permissions.
    /// Example: "System.Administrator" role granting broad access.
    /// </remarks>
    public class ApplicationRole
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the human-readable name of the role.
        /// </summary>
        /// <remarks>
        /// Example: "System Administrator" displayed in the UI.
        /// </remarks>
        public required string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the machine-readable role value.
        /// </summary>
        /// <remarks>
        /// Example: "System.Administrator" used in code logic.
        /// </remarks>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets a description of the role's purpose.
        /// </summary>
        /// <remarks>
        /// Example: "Grants full administrative access to the system."
        /// </remarks>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the role is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active role, false for a deprecated one.
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the display order for UI sorting.
        /// </summary>
        /// <remarks>
        /// Example: 1 to prioritize this role in lists.
        /// </remarks>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets when the role was created.
        /// </summary>
        /// <remarks>
        /// Example: "2023-01-15T10:00:00Z" for a role created on that date.
        /// </remarks>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the role.
        /// </summary>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets when the role was last updated.
        /// </summary>
        /// <remarks>
        /// Example: "2023-03-15T14:00:00Z" for a role updated on that date.
        /// </remarks>
        public Instant? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who last updated the role.
        /// </summary>
        public long? UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the role has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// Default is false. When true, the role is considered deleted but remains in the database.
        /// </remarks>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets when the role was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: "2023-06-01T10:00:00Z" for deletion date.
        /// </remarks>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted the role, if applicable.
        /// </summary>
        public long? DeletedByUserId { get; set; }

        public virtual ICollection<ApplicationUserRole> Users { get; set; } = [];
        public virtual ICollection<ApplicationRolePermission> Permissions { get; set; } = [];

        /// <summary>
        /// Gets or sets the user who created the role.
        /// </summary>
        public virtual ApplicationUser? CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated the role.
        /// </summary>
        public virtual ApplicationUser? UpdatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user who deleted the role, if applicable.
        /// </summary>
        public virtual ApplicationUser? DeletedByUser { get; set; }
    }
}
