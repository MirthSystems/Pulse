namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    using NodaTime;
    using Pulse.Core.Models;

    using System;

    /// <summary>
    /// Service for geographic location operations
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Geocodes a string address to geographic coordinates and timezone
        /// </summary>
        /// <param name="address">The address to geocode</param>
        /// <returns>A geocoding result with coordinates and timezone information</returns>
        Task<GeocodingResult> GeocodeAddressAsync(string address);

        /// <summary>
        /// Geocodes address components to geographic coordinates and timezone
        /// </summary>
        /// <param name="addressLine">Street address</param>
        /// <param name="locality">City or town</param>
        /// <param name="region">State or province</param>
        /// <param name="postcode">Postal code</param>
        /// <param name="country">Country</param>
        /// <returns>A geocoding result with coordinates and timezone information</returns>
        Task<GeocodingResult> GeocodeAddressComponentsAsync(
            string addressLine,
            string locality,
            string region,
            string postcode,
            string country);

        /// <summary>
        /// Gets the IANA timezone identifier for a geographic point
        /// </summary>
        /// <param name="point">Geographic point (longitude, latitude)</param>
        /// <returns>IANA timezone identifier (e.g., "America/New_York")</returns>
        Task<string> GetTimezoneForPointAsync(Point point);

        /// <summary>
        /// Converts a UTC instant to local date and time for a specific geographic location
        /// </summary>
        /// <param name="instant">The UTC instant to convert</param>
        /// <param name="point">Geographic point to determine timezone</param>
        /// <returns>Local date and time for the specified location</returns>
        Task<LocalDateTime> ConvertToLocalTimeAsync(Instant instant, Point point);

        /// <summary>
        /// Standardizes an address using Azure Maps geocoding service
        /// </summary>
        /// <param name="address">Raw address to standardize</param>
        /// <returns>Standardized address or original if standardization fails</returns>
        Task<string> StandardizeAddressAsync(string address);
    }
}
