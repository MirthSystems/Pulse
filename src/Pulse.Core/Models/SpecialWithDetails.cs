namespace Pulse.Core.Models
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Extended response model for special with all details
    /// </summary>
    public class SpecialWithDetails : SpecialItem
    {
        public List<TagItem> Tags { get; set; } = new();
        public VenueItem? Venue { get; set; }
    }
}
