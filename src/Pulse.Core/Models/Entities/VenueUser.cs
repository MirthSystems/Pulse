namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Associates a user with a venue they have access to
    /// </summary>
    public class VenueUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        public int VenueId { get; set; }
        public virtual Venue Venue { get; set; } = null!;

        /// <summary>
        /// Whether the user is the verified owner of this venue
        /// </summary>
        public bool IsVerifiedOwner { get; set; } = false;

        /// <summary>
        /// When this association was created
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// User who created this association
        /// </summary>
        public int? CreatedByUserId { get; set; }
        public virtual ApplicationUser? CreatedByUser { get; set; }

        /// <summary>
        /// The specific permissions this user has for this venue
        /// </summary>
        public virtual List<VenueUserToPermissionLink> Permissions { get; set; } = [];
    }
}
