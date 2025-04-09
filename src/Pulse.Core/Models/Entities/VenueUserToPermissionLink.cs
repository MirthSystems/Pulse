namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Links a venue-user association with a specific permission
    /// </summary>
    public class VenueUserToPermissionLink
    {
        public int VenueUserId { get; set; }
        public virtual VenueUser VenueUser { get; set; } = null!;

        public int VenuePermissionId { get; set; }
        public virtual VenuePermission Permission { get; set; } = null!;

        /// <summary>
        /// When this permission was granted
        /// </summary>
        public Instant GrantedAt { get; set; }

        /// <summary>
        /// User who granted this permission
        /// </summary>
        public int? GrantedByUserId { get; set; }
        public virtual ApplicationUser? GrantedByUser { get; set; }
    }
}
