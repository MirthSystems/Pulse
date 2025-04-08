namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a vibe descriptor created by users to describe current venue atmosphere
    /// </summary>
    /// <remarks>
    /// <para>Vibes are preceded by # symbol for visual consistency with Tags</para>
    /// <para>Examples: #busy, #quiet, #greatservice, #goodvibes, #nocover</para>
    /// </remarks>
    public class Vibe
    {
        public int Id { get; set; }

        /// <summary>
        /// The name of the vibe without the # prefix
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// How frequently this vibe has been used
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>
        /// When this vibe was first created
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Posts that use this vibe
        /// </summary>
        public List<PostVibe> Posts { get; set; } = [];
    }
}
