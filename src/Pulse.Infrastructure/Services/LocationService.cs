namespace Pulse.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Core;
    using Azure.Maps.Geolocation;
    using Azure.Maps.Search;
    using Azure.Maps.Search.Models;
    using Azure.Maps.TimeZones;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using NetTopologySuite.Geometries;
    using NodaTime;
    using NodaTime.TimeZones;
    using Pulse.Core.Contracts;

    using Pulse.Core.Models;
    using Azure;
    using Azure.Core.GeoJson;
    using Pulse.Core.Utilities;

    /// <summary>
    /// Implementation of location service using Azure Maps
    /// </summary>
    public class LocationService : ILocationService
    {
        private readonly ILogger<LocationService> _logger;
        private readonly IClock _clock;
        private readonly IDateTimeZoneProvider _dateTimeZoneProvider;
        private readonly MapsSearchClient _searchClient;
        private readonly MapsTimeZoneClient _timeZoneClient;

        // Cache for timezone lookups to reduce API calls
        private readonly Dictionary<string, string> _timezoneCache = new();
        private readonly object _cacheLock = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class using Azure Maps subscription key.
        /// </summary>
        /// <param name="azureMapsSubscriptionKey">Azure Maps subscription key</param>
        /// <param name="clock">NodaTime clock for time calculations</param>
        /// <param name="dateTimeZoneProvider">Provider for timezone data</param>
        /// <param name="logger">Logger for diagnostic information</param>
        /// <exception cref="ArgumentException">Thrown when Azure Maps subscription key is invalid or missing</exception>
        public LocationService(
            string azureMapsSubscriptionKey,
            IClock clock,
            IDateTimeZoneProvider dateTimeZoneProvider,
            ILogger<LocationService> logger)
        {
            _logger = logger;
            _clock = clock;
            _dateTimeZoneProvider = dateTimeZoneProvider;

            if (string.IsNullOrEmpty(azureMapsSubscriptionKey))
            {
                throw new ArgumentException("Azure Maps subscription key is required", nameof(azureMapsSubscriptionKey));
            }

            var credential = new AzureKeyCredential(azureMapsSubscriptionKey);
            _searchClient = new MapsSearchClient(credential);
            _timeZoneClient = new MapsTimeZoneClient(credential);
        }

        /// <summary>
        /// Geocodes a string address to geographic coordinates and timezone.
        /// </summary>
        /// <param name="address">The address to geocode</param>
        /// <returns>A geocoding result with coordinates and timezone information</returns>
        public async Task<GeocodingResult> GeocodeAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                _logger.LogWarning("Geocoding attempt with empty address");
                return new GeocodingResult { Success = false, ErrorMessage = "Address cannot be empty" };
            }

            try
            {
                _logger.LogInformation("Geocoding address: {Address}", address);

                // Call Azure Maps Search API to geocode the address
                var response = await _searchClient.GetGeocodingAsync(address);

                // Check if we have valid results
                if (response?.Value == null || response.Value.Features.Count == 0)
                {
                    _logger.LogWarning("No geocoding results found for address: {Address}", address);
                    return new GeocodingResult { Success = false, ErrorMessage = "No results found for the address" };
                }

                // Get the most relevant result (first one)
                var feature = response.Value.Features[0];

                // Create point from coordinates (longitude first, latitude second)
                var coordinates = feature.Geometry.Coordinates;
                var point = new Point(coordinates[0], coordinates[1]) { SRID = 4326 };

                // Get timezone for the location
                var timezoneId = await GetTimezoneForPointAsync(point);

                return new GeocodingResult
                {
                    Point = point,
                    TimezoneId = timezoneId,
                    Address = feature.Properties.Address,
                    Confidence = feature.Properties.Confidence,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding address: {Address}", address);
                return new GeocodingResult { Success = false, ErrorMessage = $"Failed to geocode address: {ex.Message}" };
            }
        }

        /// <summary>
        /// Geocodes address components to geographic coordinates and timezone.
        /// </summary>
        /// <param name="addressLine">Street address</param>
        /// <param name="locality">City or town</param>
        /// <param name="region">State or province</param>
        /// <param name="postcode">Postal code</param>
        /// <param name="country">Country</param>
        /// <returns>A geocoding result with coordinates and timezone information</returns>
        public async Task<GeocodingResult> GeocodeAddressComponentsAsync(
            string addressLine,
            string locality,
            string region,
            string postcode,
            string country)
        {
            try
            {
                var formattedAddress = LocationHelper.FormatAddress(
                    addressLine, locality, region, postcode, country);

                return await GeocodeAddressAsync(formattedAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding address components: {AddressLine}, {Locality}, {Region}, {Postcode}, {Country}",
                    addressLine, locality, region, postcode, country);

                return new GeocodingResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to geocode address components: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Gets the IANA timezone identifier for a geographic point.
        /// </summary>
        /// <param name="point">Geographic point (longitude, latitude)</param>
        /// <returns>IANA timezone identifier (e.g., "America/New_York")</returns>
        /// <exception cref="ArgumentNullException">Thrown when point is null</exception>
        public async Task<string> GetTimezoneForPointAsync(Point point)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            // Create cache key from coordinates (round to 3 decimal places for reasonable cache hits)
            var cacheKey = $"{Math.Round(point.X, 3)},{Math.Round(point.Y, 3)}";

            lock (_cacheLock)
            {
                if (_timezoneCache.TryGetValue(cacheKey, out var cachedTimezone))
                {
                    _logger.LogDebug("Timezone cache hit for location: ({Longitude}, {Latitude})", point.X, point.Y);
                    return cachedTimezone;
                }
            }

            try
            {
                _logger.LogInformation("Getting timezone for location: ({Longitude}, {Latitude})", point.X, point.Y);

                var position = new GeoPosition(point.Y, point.X);

                var options = new GetTimeZoneOptions();
                var response = await _timeZoneClient.GetTimeZoneByCoordinatesAsync(position, options);

                if (response?.Value == null || response.Value.TimeZones.Count == 0)
                {
                    _logger.LogWarning("No timezone found for location: ({Longitude}, {Latitude})", point.X, point.Y);
                    return "Etc/UTC";
                }

                var timezone = response.Value.TimeZones[0].Id;

                lock (_cacheLock)
                {
                    _timezoneCache[cacheKey] = timezone;
                }

                return timezone;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting timezone for location: ({Longitude}, {Latitude})", point.X, point.Y);
                return "Etc/UTC";
            }
        }

        /// <summary>
        /// Converts a UTC instant to local date and time for a specific geographic location.
        /// </summary>
        /// <param name="instant">The UTC instant to convert</param>
        /// <param name="point">Geographic point to determine timezone</param>
        /// <returns>Local date and time for the specified location</returns>
        /// <exception cref="ArgumentNullException">Thrown when point is null</exception>
        public async Task<LocalDateTime> ConvertToLocalTimeAsync(Instant instant, Point point)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            try
            {
                var timezoneId = await GetTimezoneForPointAsync(point);

                var timeZone = _dateTimeZoneProvider.GetZoneOrNull(timezoneId) ?? DateTimeZone.Utc;

                var zonedDateTime = instant.InZone(timeZone);
                return zonedDateTime.LocalDateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting time for location: ({Longitude}, {Latitude})", point.X, point.Y);
                return instant.InUtc().LocalDateTime;
            }
        }

        /// <summary>
        /// Standardizes an address using Azure Maps geocoding service.
        /// </summary>
        /// <param name="address">Raw address to standardize</param>
        /// <returns>Standardized address or original if standardization fails</returns>
        public async Task<string> StandardizeAddressAsync(string address)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                {
                    return string.Empty;
                }

                _logger.LogInformation("Standardizing address using Azure Maps: {Address}", address);

                var geocodingResult = await GeocodeAddressAsync(address);

                if (!geocodingResult.Success || geocodingResult.Address == null)
                    return address;

                return geocodingResult.Address.FormattedAddress ?? address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error standardizing address: {Address}", address);
                return address;
            }
        }
    }
}
