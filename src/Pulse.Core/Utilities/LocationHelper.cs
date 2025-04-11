namespace Pulse.Core.Utilities
{
    using Azure.Core.GeoJson;
    using Azure.Maps.Search.Models;

    using NetTopologySuite.Geometries;

    /// <summary>
    /// Helper utility for location-based operations
    /// </summary>
    public static class LocationHelper
    {
        /// <summary>
        /// Conversion factor from meters to miles (1 mile = 1609.344 meters)
        /// </summary>
        public const double MetersPerMile = 1609.344;

        /// <summary>
        /// Ensures that a point has SRID set to 4326 (WGS84)
        /// </summary>
        /// <param name="point">Geographic point to validate</param>
        /// <returns>Point with SRID set to 4326</returns>
        /// <exception cref="ArgumentNullException">Thrown when point is null</exception>
        public static Point EnsureSrid(Point point)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));

            if (point.SRID != 4326)
                return new Point(point.X, point.Y) { SRID = 4326 };

            return point;
        }

        /// <summary>
        /// Converts miles to meters
        /// </summary>
        /// <param name="miles">Distance in miles</param>
        /// <returns>Distance in meters</returns>
        public static double MilesToMeters(double miles)
        {
            return miles * MetersPerMile;
        }

        /// <summary>
        /// Creates a formatted address string from address components
        /// </summary>
        /// <param name="address">Primary address</param>
        /// <param name="locality">City/town</param>
        /// <param name="region">State/province</param>
        /// <param name="postcode">Postal/ZIP code</param>
        /// <param name="country">Country</param>
        /// <returns>Formatted address string</returns>
        public static string FormatAddress(
            string address,
            string locality,
            string region,
            string postcode,
            string country)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(address))
                parts.Add(address.Trim());

            var cityState = new List<string>();
            if (!string.IsNullOrWhiteSpace(locality))
                cityState.Add(locality.Trim());
            if (!string.IsNullOrWhiteSpace(region))
                cityState.Add(region.Trim());

            if (cityState.Any())
                parts.Add(string.Join(", ", cityState));

            if (!string.IsNullOrWhiteSpace(postcode))
                parts.Add(postcode.Trim());

            if (!string.IsNullOrWhiteSpace(country))
                parts.Add(country.Trim());

            return string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        }
    }
}
