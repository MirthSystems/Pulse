namespace MirthSystems.Pulse.Core.Models.Entities
{
    /// <summary>
    /// Represents a role that can be assigned to users within the context of a specific venue.
    /// </summary>
    /// <remarks>
    /// <para>Venue roles define permissions that apply only within the context of a specific venue.</para>
    /// <para>They allow for granular access control where users may have different levels of access to different venues.</para>
    /// </remarks>
    public class VenueRole
    {
        /// <summary>
        /// Gets or sets the unique identifier for the venue role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the role.
        /// </summary>
        /// <remarks>
        /// <para>Examples: "Venue Owner", "Venue Manager", "Specials Editor"</para>
        /// </remarks>
        public required string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the role, used programmatically.
        /// </summary>
        /// <remarks>
        /// <para>This should be a dot-notation string representing the resource and level.</para>
        /// <para>Examples: "Venue.Owner", "Venue.Manager", "Venue.SpecialsEditor"</para>
        /// </remarks>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <remarks>
        /// <para>This should clearly explain the purpose and scope of the role.</para>
        /// <para>Example: "Full control over a venue's configuration and user access"</para>
        /// </remarks>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of venue user associations assigned this role.
        /// </summary>
        public virtual ICollection<VenueUserRole> VenueUsers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of permissions assigned to this role.
        /// </summary>
        public virtual ICollection<VenueRolePermission> Permissions { get; set; } = [];

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
