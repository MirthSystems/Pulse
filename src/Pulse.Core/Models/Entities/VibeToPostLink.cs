namespace Pulse.Core.Models.Entities
{
    /// <summary>
    /// Join entity representing the many-to-many relationship between Vibes and Posts
    /// </summary>
    public class VibeToPostLink
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        public int VibeId { get; set; }
        public virtual Vibe Vibe { get; set; } = null!;
    }
}
