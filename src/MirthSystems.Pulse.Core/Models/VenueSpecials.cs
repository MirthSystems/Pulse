namespace MirthSystems.Pulse.Core.Models
{
    public class VenueSpecials
    {
        /// <summary>
        /// The venue ID these specials belong to
        /// </summary>
        /// <remarks>e.g. 5</remarks>
        public long VenueId { get; set; }

        /// <summary>
        /// The name of the venue
        /// </summary>
        /// <remarks>e.g. The Rusty Anchor Pub</remarks>
        public required string VenueName { get; set; }

        /// <summary>
        /// The list of specials associated with this venue
        /// </summary>
        public required ICollection<SpecialListItem> Specials { get; set; }
    }
}
