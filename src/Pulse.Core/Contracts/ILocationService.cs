namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    using NodaTime;
    using Pulse.Core.Models;

    using System;

    /// <summary>
    /// Service for location-based operations including geocoding, timezone calculations, and distance calculations
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Geocodes an address to obtain coordinates and timezone information
        /// </summary>
        /// <param name="address">Address string to geocode</param>
        /// <returns>GeocodingResult with coordinates and address information</returns>
        Task<GeocodingResult> GeocodeAddressAsync(string address);

        /// <summary>
        /// Geocodes address components to obtain coordinates and timezone information
        /// </summary>
        /// <param name="addressLine">Primary address line</param>
        /// <param name="locality">City or locality</param>
        /// <param name="region">State, province, or region</param>
        /// <param name="postcode">Postal code</param>
        /// <param name="country">Country</param>
        /// <returns>GeocodingResult with coordinates and address information</returns>
        Task<GeocodingResult> GeocodeAddressComponentsAsync(
            string addressLine,
            string locality,
            string region,
            string postcode,
            string country);

        /// <summary>
        /// Determines the timezone for a geographic point
        /// </summary>
        /// <param name="point">Geographic point (longitude, latitude)</param>
        /// <returns>Timezone ID in IANA format (e.g., "America/New_York")</returns>
        Task<string> GetTimezoneForPointAsync(Point point);

        /// <summary>
        /// Converts a UTC instant to local time at a specific geographic point
        /// </summary>
        /// <param name="instant">UTC instant</param>
        /// <param name="point">Geographic point (longitude, latitude)</param>
        /// <returns>Local date and time</returns>
        Task<LocalDateTime> ConvertToLocalTimeAsync(Instant instant, Point point);

        /// <summary>
        /// Standardizes an address using Azure Maps
        /// </summary>
        /// <param name="address">Raw address string</param>
        /// <returns>Standardized address string</returns>
        Task<string> StandardizeAddressAsync(string address);
    }
}
