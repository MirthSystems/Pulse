namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a permission within the application's authorization system.
    /// </summary>
    /// <remarks>
    /// Permissions define specific actions on resources.
    /// Example: "venues:edit" allows editing venue details.
    /// </remarks>
    public class ApplicationPermission
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the unique permission value.
        /// </summary>
        /// <remarks>
        /// Example: "venues:edit" for editing venue information.
        /// </remarks>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets a description of the permission.
        /// </summary>
        /// <remarks>
        /// Example: "Allows editing of venue details."
        /// </remarks>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets when the permission was created.
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the permission.
        /// </summary>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets when the permission was last updated.
        /// </summary>
        public Instant? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who last updated the permission.
        /// </summary>
        public long? UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the permission is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active permission, false for a deprecated one.
        /// </remarks>
        public bool IsActive { get; set; } = true; 

        public virtual ICollection<ApplicationRolePermission> Roles { get; set; } = [];
        public virtual ICollection<ApplicationUserPermission> Users { get; set; } = [];
        public virtual ICollection<VenueRolePermission> VenueRoles { get; set; } = [];

        /// <summary>
        /// Gets or sets the user who created the permission.
        /// </summary>
        public virtual ApplicationUser? CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated the permission.
        /// </summary>
        public virtual ApplicationUser? UpdatedByUser { get; set; }
    }
}
