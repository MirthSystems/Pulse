namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Response model for a venue in a list
    /// </summary>
    public class VenueListItem
    {
        /// <summary>
        /// The unique identifier for the venue
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The name of the venue
        /// </summary>
        /// <remarks>e.g. The Rusty Anchor Pub</remarks>
        public required string Name { get; set; }

        /// <summary>
        /// Description of the venue and its offerings
        /// </summary>
        /// <remarks>e.g. A cozy pub with live music and craft beers.</remarks>
        public string? Description { get; set; }

        /// <summary>
        /// The city or town
        /// </summary>
        /// <remarks>e.g. Springfield</remarks>
        public required string Locality { get; set; }

        /// <summary>
        /// The state, province, or region
        /// </summary>
        /// <remarks>e.g. Illinois</remarks>
        public required string Region { get; set; }

        /// <summary>
        /// URL to the venue's profile image
        /// </summary>
        /// <remarks>e.g. https://cdn.pulse.com/venues/123456/profile.jpg</remarks>
        public string? ProfileImage { get; set; }

        /// <summary>
        /// The venue's latitude coordinate
        /// </summary>
        /// <remarks>e.g. 41.8781</remarks>
        public double? Latitude { get; set; }

        /// <summary>
        /// The venue's longitude coordinate
        /// </summary>
        /// <remarks>e.g. -87.6298</remarks>
        public double? Longitude { get; set; }
    }
}
