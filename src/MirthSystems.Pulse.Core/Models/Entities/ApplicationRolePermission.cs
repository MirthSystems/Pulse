namespace MirthSystems.Pulse.Core.Models.Entities
{

    /// <summary>
    /// Represents the many-to-many relationship between application roles and permissions.
    /// </summary>
    /// <remarks>
    /// <para>This join entity maps which permissions belong to which application roles.</para>
    /// <para>It allows a role to have multiple permissions and a permission to be part of multiple roles.</para>
    /// </remarks>
    public class ApplicationRolePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier for the role-permission association.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the ApplicationRole entity.
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the Permission entity.
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated ApplicationRole.
        /// </summary>
        public required virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated Permission.
        /// </summary>
        public required virtual ApplicationPermission Permission { get; set; }
    }
}
