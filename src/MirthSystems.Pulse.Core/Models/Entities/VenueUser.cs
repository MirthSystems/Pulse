namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the association between a user and a venue.
    /// </summary>
    /// <remarks>
    /// Essential for venue-specific roles and permissions.
    /// Example: User "auth0|12345" associated with "The Rusty Anchor Pub".
    /// </remarks>
    public class VenueUser
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long VenueId { get; set; }

        /// <summary>
        /// Gets or sets when the association was created.
        /// </summary>
        /// <remarks>
        /// Example: "2023-03-01T09:00:00Z" for association date.
        /// </remarks>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the association.
        /// </summary>
        /// <remarks>
        /// Example: 1 for the system admin who added the user.
        /// </remarks>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the association is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active association.
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the association has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// Default is false. When true, the association is considered deleted but remains in the database.
        /// </remarks>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets when the association was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: "2023-06-01T10:00:00Z" for deletion date.
        /// </remarks>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted the association, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: 3 for the admin who removed it.
        /// </remarks>
        public long? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        /// <remarks>
        /// Example: User with UserObjectId "auth0|12345".
        /// </remarks>
        public required virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the associated venue.
        /// </summary>
        /// <remarks>
        /// Example: Venue named "The Rusty Anchor Pub".
        /// </remarks>
        public required virtual Venue Venue { get; set; }

        /// <summary>
        /// Gets or sets the user who created the association.
        /// </summary>
        /// <remarks>
        /// Optional navigation property; null if created by the system.
        /// </remarks>
        public virtual ApplicationUser? CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user who deleted the association, if applicable.
        /// </summary>
        /// <remarks>
        /// Optional navigation property; null if not deleted or deleted by the system.
        /// </remarks>
        public virtual ApplicationUser? DeletedByUser { get; set; }

        public virtual ICollection<VenueUserRole> Roles { get; set; } = [];
    }
}
