namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the association between a user and a venue, including their roles within that venue.
    /// </summary>
    /// <remarks>
    /// <para>This is a join entity that creates a many-to-many relationship between users and venues with role information.</para>
    /// <para>Each user can be associated with multiple venues, and each venue can have multiple users.</para>
    /// <para>Each user-venue relationship can have multiple roles (e.g., Owner, Manager) through the VenueUserRoles collection.</para>
    /// </remarks>
    public class VenueUser
    {
        /// <summary>
        /// Gets or sets the unique identifier for the venue-user association.
        /// </summary>
        /// <remarks>
        /// This is the primary key for the VenueUser entity in the database.
        /// </remarks>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the ApplicationUser entity.
        /// </summary>
        /// <remarks>
        /// <para>This represents the user who has a role within the venue.</para>
        /// <para>This is a required field and is used to establish the relationship with the ApplicationUser entity.</para>
        /// </remarks>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the Venue entity.
        /// </summary>
        /// <remarks>
        /// <para>This represents the venue where the user has a role.</para>
        /// <para>This is a required field and is used to establish the relationship with the Venue entity.</para>
        /// </remarks>
        public long VenueId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this user was first associated with this venue.
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created this association.
        /// </summary>
        /// <remarks>
        /// <para>This provides an audit trail for venue user assignments.</para>
        /// <para>If null, the association was created by the system.</para>
        /// </remarks>
        public long? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user-venue association is active.
        /// </summary>
        /// <remarks>
        /// <para>When false, all permissions for this user at this venue are effectively revoked.</para>
        /// <para>This allows temporarily suspending access without removing role assignments.</para>
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the navigation property to the associated ApplicationUser.
        /// </summary>
        /// <remarks>
        /// <para>This is the navigation property that links to the user associated with this venue.</para>
        /// <para>It provides access to the user's information such as their name, email, and other profile details.</para>
        /// </remarks>
        public required virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated Venue.
        /// </summary>
        /// <remarks>
        /// <para>This is the navigation property that links to the venue associated with this user.</para>
        /// <para>It provides access to the venue's information such as its name, address, and other details.</para>
        /// </remarks>
        public required virtual Venue Venue { get; set; }

        /// <summary>
        /// Gets or sets the join entity collection that establishes the many-to-many relationship between VenueUser and VenueRole.
        /// </summary>
        /// <remarks>
        /// <para>This collection connects to the VenueUserRole join table in the database.</para>
        /// <para>It represents the explicit relationships between this user-venue pair and all assigned roles.</para>
        /// <para>Use this collection for adding or removing roles from this user-venue relationship.</para>
        /// </remarks>
        public virtual ICollection<VenueUserRole> VenueRoles { get; set; } = [];
    }
}
