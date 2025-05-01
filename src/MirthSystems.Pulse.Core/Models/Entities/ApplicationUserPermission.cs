namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;
    /// <summary>
    /// Represents a direct assignment of a permission to a user, bypassing roles.
    /// </summary>
    /// <remarks>
    /// <para>This provides fine-grained control over user permissions beyond role-based assignments.</para>
    /// <para>Direct permissions can be used to grant or deny specific capabilities to individual users.</para>
    /// </remarks>
    public class ApplicationUserPermission
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user-permission association.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the ApplicationUser entity.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the Permission entity.
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated ApplicationUser.
        /// </summary>
        public required virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated Permission.
        /// </summary>
        public required virtual ApplicationPermission Permission { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this permission was assigned.
        /// </summary>
        public Instant AssignedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who assigned this permission.
        /// </summary>
        /// <remarks>
        /// <para>This provides an audit trail for permission assignments.</para>
        /// <para>If null, the permission was assigned by the system.</para>
        /// </remarks>
        public long? AssignedByUserId { get; set; }
    }
}
