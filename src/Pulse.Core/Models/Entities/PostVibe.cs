namespace Pulse.Core.Models.Entities
{
    public class PostVibe
    {
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;

        public int VibeId { get; set; }
        public Vibe Vibe { get; set; } = null!;
    }
}
