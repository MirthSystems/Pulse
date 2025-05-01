namespace MirthSystems.Pulse.Core.Models.Entities
{
    /// <summary>
    /// Represents a permission within the application's authorization system.
    /// </summary>
    /// <remarks>
    /// <para>Permissions are the fundamental units of authorization in the system and represent
    /// discrete actions that can be performed on resources.</para>
    /// <para>They can be assigned directly to users or grouped into roles.</para>
    /// </remarks>
    public class ApplicationPermission
    {
        /// <summary>
        /// Gets or sets the unique identifier for the permission.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the permission, used programmatically.
        /// </summary>
        /// <remarks>
        /// <para>This should be a colon-notation string representing the resource and action.</para>
        /// <para>Examples: "specials:create", "venues:edit", "users:manage"</para>
        /// </remarks>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the permission.
        /// </summary>
        /// <remarks>
        /// <para>This should clearly explain what the permission allows.</para>
        /// <para>Examples: "Allows creating new specials", "Allows editing venue details"</para>
        /// </remarks>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of application roles that include this permission.
        /// </summary>
        public virtual ICollection<ApplicationRolePermission> Roles { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of users that have been directly assigned this permission.
        /// </summary>
        public virtual ICollection<ApplicationUserPermission> Users { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of venue roles that include this permission.
        /// </summary>
        public virtual ICollection<VenueRolePermission> VenueRoles { get; set; } = [];
    }
}
