namespace MirthSystems.Pulse.Core.Models.Entities
{
    using System.Security;
    using NodaTime;

    /// <summary>
    /// Represents the many-to-many relationship between venue roles and permissions.
    /// </summary>
    /// <remarks>
    /// Links venue roles to specific permissions.
    /// Example: "Venue.Manager" linked to "specials:edit".
    /// </remarks>
    public class VenueRolePermission
    {
        public long Id { get; set; }
        public long VenueRoleId { get; set; }
        public long PermissionId { get; set; }
        
        /// <summary>
        /// Gets or sets when the permission was assigned to the venue role.
        /// </summary>
        /// <remarks>
        /// Example: "2023-02-01T14:00:00Z" for a permission assigned on February 1, 2023.
        /// </remarks>
        public Instant CreatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the user who assigned the permission to the venue role.
        /// </summary>
        /// <remarks>
        /// This field is nullable; null indicates the assignment was done by the system.
        /// </remarks>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the associated venue role.
        /// </summary>
        /// <remarks>
        /// Example: Role with Value "Venue.Manager".
        /// </remarks>
        public required virtual VenueRole VenueRole { get; set; }

        /// <summary>
        /// Gets or sets the associated permission.
        /// </summary>
        /// <remarks>
        /// Example: Permission with Value "specials:edit".
        /// </remarks>
        public required virtual ApplicationPermission Permission { get; set; }
        
        /// <summary>
        /// Gets or sets the user who created this role-permission association.
        /// </summary>
        /// <remarks>
        /// Optional navigation property; null if the association was created by the system.
        /// </remarks>
        public virtual ApplicationUser? CreatedByUser { get; set; }
    }
}
