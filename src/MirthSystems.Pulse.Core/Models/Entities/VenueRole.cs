namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a role specific to a venue.
    /// </summary>
    /// <remarks>
    /// Defines permissions within a venue's context.
    /// Example: "Venue.Manager" for managing a specific venue.
    /// </remarks>
    public class VenueRole
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the human-readable name of the role.
        /// </summary>
        /// <remarks>
        /// Example: "Venue Manager" displayed in the UI.
        /// </remarks>
        public required string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the machine-readable role value.
        /// </summary>
        /// <remarks>
        /// Example: "Venue.Manager" used in code logic.
        /// </remarks>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets a description of the role's purpose.
        /// </summary>
        /// <remarks>
        /// Example: "Manages venue operations and specials."
        /// </remarks>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the display order for UI sorting.
        /// </summary>
        /// <remarks>
        /// Example: 2 to order this role in lists.
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
        /// <remarks>
        /// This field is nullable; null indicates the role was created by the system.
        /// </remarks>
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
        /// Gets or sets whether the role is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active role.
        /// </remarks>
        public bool IsActive { get; set; } = true; 

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

        public virtual ICollection<VenueUserRole> VenueUsers { get; set; } = [];
        public virtual ICollection<VenueRolePermission> Permissions { get; set; } = [];

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
