namespace Pulse.Core.Models
{
    using Azure.Maps.Search.Models;

    using NetTopologySuite.Geometries;

    /// <summary>
    /// Result of a geocoding operation
    /// </summary>
    public class GeocodingResult
    {
        /// <summary>
        /// Geographic point (longitude, latitude)
        /// </summary>
        public Point? Point { get; set; }

        /// <summary>
        /// Timezone ID in IANA format (e.g., "America/New_York")
        /// </summary>
        public string? TimezoneId { get; set; }

        /// <summary>
        /// Azure Maps address object with detailed address components
        /// </summary>
        public Address? Address { get; set; }

        /// <summary>
        /// Confidence level of the geocoding result
        /// </summary>
        public ConfidenceEnum? Confidence { get; set; }

        /// <summary>
        /// Whether the geocoding was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error message if geocoding failed
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
