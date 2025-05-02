namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a direct assignment of a permission to a user.
    /// </summary>
    /// <remarks>
    /// <para>This entity allows permissions to be granted directly to users, bypassing role assignments for fine-grained control.</para>
    /// <para>Example: Granting the "venues:edit" permission directly to a user with UserObjectId "auth0|12345".</para>
    /// </remarks>
    public class ApplicationUserPermission
    {
        /// <summary>
        /// Gets or sets the unique identifier for this user-permission assignment.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated user.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated permission.
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the permission was assigned.
        /// </summary>
        /// <remarks>
        /// <para>Uses NodaTime's Instant for precise timestamping.</para>
        /// <para>Example: "2023-02-01T14:00:00Z" for a permission assigned on February 1, 2023, at 2:00 PM UTC.</para>
        /// </remarks>
        public Instant AssignedAt { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the user who assigned this permission, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>This field is nullable; null indicates the permission was assigned by the system.</para>
        /// <para>Example: 2 for the admin user who granted the permission.</para>
        /// </remarks>
        public long? AssignedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the permission assignment is active.
        /// </summary>
        /// <remarks>
        /// Example: true for an active assignment, false for a suspended one.
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the permission assignment has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// Default is false. When true, the assignment is considered deleted but remains in the database.
        /// </remarks>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets when the permission assignment was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// Example: "2023-06-01T10:00:00Z" for deletion date.
        /// </remarks>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted the permission assignment, if applicable.
        /// </summary>
        public long? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who is assigned this permission.
        /// </summary>
        /// <remarks>
        /// <para>Required navigation property linking to the ApplicationUser entity.</para>
        /// <para>Example: User with UserObjectId "auth0|12345".</para>
        /// </remarks>
        public required virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the permission assigned to the user.
        /// </summary>
        /// <remarks>
        /// <para>Required navigation property linking to the ApplicationPermission entity.</para>
        /// <para>Example: Permission with Value "venues:edit".</para>
        /// </remarks>
        public required virtual ApplicationPermission Permission { get; set; }

        /// <summary>
        /// Gets or sets the user who assigned this permission, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Optional navigation property; null if the permission was assigned by the system.</para>
        /// <para>Example: User with UserObjectId "auth0|67890" who assigned the permission.</para>
        /// </remarks>
        public virtual ApplicationUser? AssignedByUser { get; set; }

        /// <summary>
        /// Gets or sets the user who deleted this permission assignment, if applicable.
        /// </summary>
        public virtual ApplicationUser? DeletedByUser { get; set; }
    }
}
