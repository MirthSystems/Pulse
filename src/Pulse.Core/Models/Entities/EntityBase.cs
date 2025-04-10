namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Base class for all entities with common audit fields
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// When this entity was created
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Auth0 ID of the user who created this entity
        /// </summary>
        public string? CreatedByUserId { get; set; }

        /// <summary>
        /// When this entity was last updated
        /// </summary>
        public Instant? UpdatedAt { get; set; }

        /// <summary>
        /// Auth0 ID of the user who last updated this entity
        /// </summary>
        public string? UpdatedByUserId { get; set; }
    }
}
