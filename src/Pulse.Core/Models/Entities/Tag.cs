namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents a tag that can be applied to specials for discovery and categorization
    /// </summary>
    /// <remarks>
    /// <para>Tags are preceded by # symbol for visual distinction</para>
    /// <para>Examples: #happyhour, #wingsnight, #liveband, #djset, #karaoke</para>
    /// </remarks>
    public class Tag
    {
        public int Id { get; set; }

        /// <summary>
        /// The name of the tag without the # prefix
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// How frequently this tag has been used
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>
        /// When this tag was first created
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Specials that use this tag
        /// </summary>
        public virtual List<TagToSpecialLink> Specials { get; set; } = [];
    }
}
