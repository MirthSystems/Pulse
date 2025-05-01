namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the join entity connecting a VenueUser to a VenueRole, establishing the many-to-many relationship.
    /// </summary>
    /// <remarks>
    /// <para>This join entity allows a user to have multiple roles within a specific venue.</para>
    /// <para>It creates explicit connections between a user-venue pair (VenueUser) and the roles assigned to that relationship.</para>
    /// <para>For example, a user might be both an "Owner" and "Manager" of a venue, requiring two VenueUserRole records.</para>
    /// </remarks>
    public class VenueUserRole
    {
        /// <summary>
        /// Gets or sets the unique identifier for this venue-user-role association.
        /// </summary>
        /// <remarks>
        /// This is the primary key for the VenueUserRole entity in the database.
        /// </remarks>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the VenueUser entity.
        /// </summary>
        /// <remarks>
        /// <para>This links to the VenueUser that represents a specific user-venue relationship.</para>
        /// <para>This is a required field and is used to establish the relationship with the VenueUser entity.</para>
        /// </remarks>
        public long VenueUserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the VenueRole entity.
        /// </summary>
        /// <remarks>
        /// <para>This represents the role being assigned to the user for the specific venue.</para>
        /// <para>This is a required field and is used to establish the relationship with the VenueRole entity.</para>
        /// </remarks>
        public long VenueRoleId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this role was assigned.
        /// </summary>
        public Instant AssignedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who assigned this role.
        /// </summary>
        /// <remarks>
        /// <para>This provides an audit trail for role assignments.</para>
        /// <para>If null, the role was assigned by the system.</para>
        /// </remarks>
        public long? AssignedByUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this role assignment is active.
        /// </summary>
        /// <remarks>
        /// <para>When false, the role is considered temporarily suspended without removing the assignment.</para>
        /// <para>This allows for quick reactivation without recreating the association.</para>
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this role assignment is soft deleted.
        /// </summary>
        /// <remarks>
        /// <para>When true, the record should be excluded from normal queries but is retained for audit history.</para>
        /// <para>Soft deletion preserves the historical record of role assignments.</para>
        /// </remarks>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when this role assignment was soft deleted, if applicable.
        /// </summary>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted this role assignment, if applicable.
        /// </summary>
        public long? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated VenueUser entity.
        /// </summary>
        /// <remarks>
        /// <para>This navigation property links to the VenueUser that this role is assigned to.</para>
        /// <para>Through this property, you can access both the user and venue information.</para>
        /// </remarks>
        public required virtual VenueUser VenueUser { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated VenueRole entity.
        /// </summary>
        /// <remarks>
        /// <para>This navigation property links to the role that is assigned to the user-venue relationship.</para>
        /// <para>Typical venue roles include "Venue Owner" and "Venue Manager" with their associated permissions.</para>
        /// </remarks>
        public required virtual VenueRole VenueRole { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the user who assigned this role, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>This provides a reference to the user who performed the assignment for audit purposes.</para>
        /// <para>May be null if the assignment was performed by the system or during initial data seeding.</para>
        /// </remarks>
        public virtual ApplicationUser? AssignedByUser { get; set; }
        
        /// <summary>
        /// Gets or sets the navigation property to the user who deleted this role, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>This provides a reference to the user who performed the deletion for audit purposes.</para>
        /// <para>May be null if the deletion was performed by the system or if the record is not deleted.</para>
        /// </remarks>
        public virtual ApplicationUser? DeletedByUser { get; set; }
    }
}
