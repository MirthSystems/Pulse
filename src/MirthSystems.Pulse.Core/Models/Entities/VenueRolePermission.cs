namespace MirthSystems.Pulse.Core.Models.Entities
{
    using System.Security;

    /// <summary>
    /// Represents the many-to-many relationship between venue roles and permissions.
    /// </summary>
    /// <remarks>
    /// <para>This join entity maps which permissions belong to which venue roles.</para>
    /// <para>It allows a venue role to have multiple permissions and a permission to be part of multiple venue roles.</para>
    /// </remarks>
    public class VenueRolePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier for the role-permission association.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the VenueRole entity.
        /// </summary>
        public long VenueRoleId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the Permission entity.
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated VenueRole.
        /// </summary>
        public required virtual VenueRole VenueRole { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated Permission.
        /// </summary>
        public required virtual ApplicationPermission Permission { get; set; }
    }
}
