namespace Pulse.Core.Models
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Extended response model for venue with all details
    /// </summary>
    public class VenueWithDetails : VenueItem
    {
        public List<OperatingScheduleItem> BusinessHours { get; set; } = new();
        public List<SpecialItem> Specials { get; set; } = new();
    }
}
