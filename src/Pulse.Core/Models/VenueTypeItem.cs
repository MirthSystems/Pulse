namespace Pulse.Core.Models
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Response model for venue type information
    /// </summary>
    public class VenueTypeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
