namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the many-to-many relationship between application roles and permissions.
    /// </summary>
    /// <remarks>
    /// Links roles to permissions for system-wide authorization.
    /// Example: "System.Administrator" linked to "venues:edit".
    /// </remarks>
    public class ApplicationRolePermission
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        
        /// <summary>
        /// Gets or sets when the permission was assigned to the role.
        /// </summary>
        /// <remarks>
        /// Example: "2023-02-01T14:00:00Z" for a permission assigned on February 1, 2023.
        /// </remarks>
        public Instant CreatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the user who assigned the permission to the role.
        /// </summary>
        /// <remarks>
        /// This field is nullable; null indicates the assignment was done by the system.
        /// </remarks>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the associated role.
        /// </summary>
        /// <remarks>
        /// Example: Role with Value "System.Administrator".
        /// </remarks>
        public required virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Gets or sets the associated permission.
        /// </summary>
        /// <remarks>
        /// Example: Permission with Value "venues:edit".
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
