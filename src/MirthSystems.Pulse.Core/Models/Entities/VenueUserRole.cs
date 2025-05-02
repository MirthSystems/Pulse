namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the many-to-many relationship between VenueUser and VenueRole.
    /// </summary>
    /// <remarks>
    /// Assigns venue-specific roles to users for a venue.
    /// Example: User assigned "Venue.Manager" for "The Rusty Anchor Pub".
    /// </remarks>
    public class VenueUserRole
    {
        public long Id { get; set; }
        public long VenueUserId { get; set; }
        public long VenueRoleId { get; set; }

        /// <summary>
        /// Gets or sets when the role was assigned.
        /// </summary>
        /// <remarks>
        /// Example: "2023-03-05T12:00:00Z" for assignment date.
        /// </remarks>
        public Instant AssignedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who assigned the role.
        /// </summary>
        /// <remarks>
        /// Example: 1 for the admin who assigned it.
        /// </remarks>
        public long? AssignedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the role assignment is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active assignment.
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the role assignment is deleted.
        /// </summary>
        /// <remarks>
        /// Example: false for an active assignment, true for a removed one.
        /// </remarks>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets when the role was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: "2023-06-01T10:00:00Z" for deletion date.
        /// </remarks>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted the role, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: 3 for the admin who removed it.
        /// </remarks>
        public long? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the venue-user association.
        /// </summary>
        /// <remarks>
        /// Example: User "auth0|12345" at "The Rusty Anchor Pub".
        /// </remarks>
        public required virtual VenueUser VenueUser { get; set; }

        /// <summary>
        /// Gets or sets the assigned venue role.
        /// </summary>
        /// <remarks>
        /// Example: Role with Value "Venue.Manager".
        /// </remarks>
        public required virtual VenueRole VenueRole { get; set; }

        public virtual ApplicationUser? AssignedByUser { get; set; }
        public virtual ApplicationUser? DeletedByUser { get; set; }
    }
}
