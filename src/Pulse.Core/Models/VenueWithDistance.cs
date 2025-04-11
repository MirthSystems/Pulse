namespace Pulse.Core.Models
{
    using NetTopologySuite.Geometries;

    using NodaTime;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Venue with distance information from a search point
    /// </summary>
    public class VenueWithDistance
    {
        /// <summary>
        /// Venue information
        /// </summary>
        public Venue Venue { get; set; } = null!;

        /// <summary>
        /// Distance from search point in miles
        /// </summary>
        public double DistanceMiles { get; set; }

        /// <summary>
        /// Search point used to calculate distance
        /// </summary>
        public Point SearchPoint { get; set; } = null!;

        /// <summary>
        /// Local time at the search location
        /// </summary>
        public LocalDateTime? LocalTime { get; set; }
    }
}
