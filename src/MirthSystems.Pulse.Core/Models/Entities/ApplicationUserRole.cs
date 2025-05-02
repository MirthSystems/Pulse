namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the many-to-many relationship between users and application roles.
    /// </summary>
    /// <remarks>
    /// <para>This entity assigns application-wide roles to users, enabling role-based access control.</para>
    /// <para>Example: Assigning the "System.Administrator" role to a user with UserObjectId "auth0|12345".</para>
    /// </remarks>
    public class ApplicationUserRole
    {
        /// <summary>
        /// Gets or sets the unique identifier for this user-role assignment.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated user.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated role.
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the role was assigned.
        /// </summary>
        /// <remarks>
        /// <para>Uses NodaTime's Instant for precise timestamping.</para>
        /// <para>Example: "2023-02-01T14:00:00Z" for a role assigned on February 1, 2023, at 2:00 PM UTC.</para>
        /// </remarks>
        public Instant AssignedAt { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the user who assigned this role, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>This field is nullable; null indicates the role was assigned by the system.</para>
        /// <para>Example: 2 for the admin user who assigned the role.</para>
        /// </remarks>
        public long? AssignedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the role assignment is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active assignment, false for a suspended one.
        /// </remarks>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Gets or sets whether the role assignment has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// Default is false. When true, the assignment is considered deleted but remains in the database.
        /// </remarks>
        public bool IsDeleted { get; set; } = false;
        
        /// <summary>
        /// Gets or sets when the role assignment was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: "2023-06-01T10:00:00Z" for deletion date.
        /// </remarks>
        public Instant? DeletedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the user who deleted the role assignment, if applicable.
        /// </summary>
        public long? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who is assigned this role.
        /// </summary>
        /// <remarks>
        /// <para>Required navigation property linking to the ApplicationUser entity.</para>
        /// <para>Example: User with UserObjectId "auth0|12345".</para>
        /// </remarks>
        public required virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the role assigned to the user.
        /// </summary>
        /// <remarks>
        /// <para>Required navigation property linking to the ApplicationRole entity.</para>
        /// <para>Example: Role with Value "System.Administrator".</para>
        /// </remarks>
        public required virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Gets or sets the user who assigned this role, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Optional navigation property; null if the role was assigned by the system.</para>
        /// <para>Example: User with UserObjectId "auth0|67890" who assigned the role.</para>
        /// </remarks>
        public virtual ApplicationUser? AssignedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user who deleted this role assignment, if applicable.
        /// </summary>
        public virtual ApplicationUser? DeletedByUser { get; set; }
    }
}
