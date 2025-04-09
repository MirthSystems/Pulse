namespace Pulse.Core.Models.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a permission that can be granted to users for specific venues
    /// </summary>
    public class VenuePermission
    {
        public int Id { get; set; }

        /// <summary>
        /// The name of the permission in format similar to Auth0 (e.g., manage:specials)
        /// This is used programmatically to check permissions
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Human-readable description of what this permission allows
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Users who have been granted this permission for specific venues
        /// </summary>
        public virtual List<VenueUserToPermissionLink> VenueUsers { get; set; } = [];
    }
}
