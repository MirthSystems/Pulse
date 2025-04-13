namespace Pulse.Core.Models
{
    /// <summary>
    /// Response model for tag information
    /// </summary>
    public class TagItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int UsageCount { get; set; }
    }
}
