namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the many-to-many relationship between users and application roles.
    /// </summary>
    /// <remarks>
    /// <para>This join entity maps which application roles are assigned to which users.</para>
    /// <para>It allows a user to have multiple roles and a role to be assigned to multiple users.</para>
    /// </remarks>
    public class ApplicationUserRole
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user-role association.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the ApplicationUser entity.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the ApplicationRole entity.
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated ApplicationUser.
        /// </summary>
        public required virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated ApplicationRole.
        /// </summary>
        public required virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this role was assigned.
        /// </summary>
        public Instant AssignedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who assigned this role.
        /// </summary>
        /// <remarks>
        /// <para>This provides an audit trail for role assignments.</para>
        /// <para>If null, the role was assigned by the system.</para>
        /// </remarks>
        public long? AssignedByUserId { get; set; }
    }
}
