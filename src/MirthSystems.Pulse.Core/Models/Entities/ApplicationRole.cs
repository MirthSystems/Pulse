namespace MirthSystems.Pulse.Core.Models.Entities
{
    using System.Security.Claims;

    /// <summary>
    /// Represents a role within the application's authorization system that can be assigned to users.
    /// </summary>
    /// <remarks>
    /// <para>Application roles apply system-wide and are not tied to specific venues.</para>
    /// <para>They group permissions to facilitate easier access management.</para>
    /// </remarks>
    public class ApplicationRole
    {
        /// <summary>
        /// Gets or sets the unique identifier for the role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the role.
        /// </summary>
        /// <remarks>
        /// <para>Examples: "System Administrator", "Content Manager"</para>
        /// </remarks>
        public required string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the role, used programmatically.
        /// </summary>
        /// <remarks>
        /// <para>This should be a dot-notation string representing the role hierarchy.</para>
        /// <para>Examples: "System.Administrator", "Content.Manager"</para>
        /// </remarks>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <remarks>
        /// <para>This should clearly explain the purpose and scope of the role.</para>
        /// <para>Example: "Full administrative access to all system functions"</para>
        /// </remarks>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of users assigned to this role.
        /// </summary>
        public virtual ICollection<ApplicationUserRole> Users { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of permissions assigned to this role.
        /// </summary>
        public virtual ICollection<ApplicationRolePermission> Permissions { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether this role is active and available for assignment.
        /// </summary>
        /// <remarks>
        /// <para>When false, this role cannot be newly assigned to users but existing assignments remain intact.</para>
        /// <para>This allows deprecating roles while preserving existing assignments.</para>
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the display order for this role when showing in lists.
        /// </summary>
        /// <remarks>
        /// <para>Lower values appear first in the list.</para>
        /// <para>System roles typically have lower values than custom roles.</para>
        /// </remarks>
        public int DisplayOrder { get; set; }
    }
}
