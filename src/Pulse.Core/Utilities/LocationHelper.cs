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
        /// Earth radius in kilometers
        /// </summary>
        private const double EarthRadiusKm = 6371.0;

        /// <summary>
        /// Conversion factor from kilometers to miles
        /// </summary>
        private const double KmToMilesFactor = 0.621371;

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
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        /// <summary>
        /// Converts meters to miles
        /// </summary>
        /// <param name="meters">Distance in meters</param>
        /// <returns>Distance in miles</returns>
        public static double MetersToMiles(double meters)
        {
            return meters / MetersPerMile;
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
        /// Calculates the distance between two geographic points using the Haversine formula
        /// </summary>
        /// <param name="point1">First point (longitude, latitude)</param>
        /// <param name="point2">Second point (longitude, latitude)</param>
        /// <returns>Distance in miles</returns>
        /// <exception cref="ArgumentNullException">Thrown when either point is null</exception>
        public static double CalculateDistanceInMiles(Point point1, Point point2)
        {
            if (point1 == null || point2 == null)
                throw new ArgumentNullException(point1 == null ? nameof(point1) : nameof(point2));

            // Ensure SRID is set to 4326 (WGS84)
            point1 = EnsureSrid(point1);
            point2 = EnsureSrid(point2);

            // Calculate distance using the Haversine formula
            var lat1 = DegreesToRadians(point1.Y);
            var lon1 = DegreesToRadians(point1.X);
            var lat2 = DegreesToRadians(point2.Y);
            var lon2 = DegreesToRadians(point2.X);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distanceKm = EarthRadiusKm * c;

            // Convert to miles
            return distanceKm * KmToMilesFactor;
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
