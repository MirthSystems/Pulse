namespace Pulse.Core.Models
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Response model for special with venue information
    /// </summary>
    public class SpecialWithVenue : SpecialItem
    {
        public VenueItem? Venue { get; set; }
    }
}
