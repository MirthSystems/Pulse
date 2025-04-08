namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents an ephemeral user post about a venue that expires after 15 minutes
    /// </summary>
    public class Post
    {
        public int Id { get; set; }

        /// <summary>
        /// The user who created this post
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The venue this post is about
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// The text content of the post
        /// </summary>
        public string? TextContent { get; set; }

        /// <summary>
        /// URL to an image the user uploaded with the post
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// URL to a short video the user uploaded with the post
        /// </summary>
        public string? VideoUrl { get; set; }

        /// <summary>
        /// When this post was created
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// When this post will expire (15 minutes after creation)
        /// </summary>
        public Instant ExpiresAt { get; set; }

        /// <summary>
        /// Flag indicating if the post has expired
        /// </summary>
        /// <remarks>
        /// This field is used for more efficient querying of expired posts
        /// without constantly comparing current time with ExpiresAt
        /// </remarks>
        public bool IsExpired { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public Venue Venue { get; set; } = null!;

        /// <summary>
        /// Vibes associated with this post
        /// </summary>
        public List<PostVibe> Vibes { get; set; } = [];
    }
}
