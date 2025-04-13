namespace Pulse.Core.Models
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Response model for venues with active specials and distance info
    /// </summary>
    public class VenueWithActiveSpecials : VenueItem
    {
        public double DistanceMiles { get; set; }
        public List<SpecialItem> ActiveSpecials { get; set; } = new();
    }
}
