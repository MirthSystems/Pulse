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
    using Pulse.Core.Models.Options;

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

        public async Task<GeocodingResult> GeocodeAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return new GeocodingResult
                {
                    Success = false,
                    ErrorMessage = "Address cannot be empty"
                };
            }

            try
            {
                _logger.LogInformation("Geocoding address: {Address}", address);

                // Call Azure Maps to geocode the address
                var response = await _searchClient.GetGeocodingAsync(query: address);

                // Check if we have valid results
                if (response?.Value == null)
                {
                    _logger.LogWarning("No geocoding results found for address: {Address}", address);
                    return new GeocodingResult
                    {
                        Success = false,
                        ErrorMessage = "No results found for the address"
                    };
                }

                // Access the first result - we need to inspect the actual structure of GeocodingResponse
                // Since GeocodingResponse doesn't have a Results property, we need to access the correct property
                if (response.Value.Summary?.NumResults <= 0)
                {
                    _logger.LogWarning("No geocoding results found for address: {Address}", address);
                    return new GeocodingResult
                    {
                        Success = false,
                        ErrorMessage = "No results found for the address"
                    };
                }

                // Get the first result from the appropriate collection
                // Note: We need to check the actual structure of GeocodingResponse to find the right collection
                var searchResult = response.Value.Summary?.NumResults > 0 ? response.Value.Addresses[0] : null;

                if (searchResult == null)
                {
                    _logger.LogWarning("No valid result in geocoding response for address: {Address}", address);
                    return new GeocodingResult
                    {
                        Success = false,
                        ErrorMessage = "No valid result in geocoding response"
                    };
                }

                // Extract position data
                var position = new GeoPosition(
                    searchResult.Position.Latitude,
                    searchResult.Position.Longitude);

                // Create point using longitude (x) and latitude (y)
                var point = new Point(position.Longitude, position.Latitude)
                {
                    SRID = 4326 // WGS 84
                };

                // Get timezone for the location
                var timezoneId = await GetTimezoneForPointAsync(point);

                return new GeocodingResult
                {
                    Point = point,
                    TimezoneId = timezoneId,
                    Address = searchResult.Address,
                    ConfidenceScore = searchResult.Score,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding address: {Address}", address);
                return new GeocodingResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to geocode address: {ex.Message}"
                };
            }
        }

        public async Task<GeocodingResult> GeocodeAddressComponentsAsync(
            string addressLine,
            string locality,
            string region,
            string postcode,
            string country)
        {
            try
            {
                // Build formatted address from components
                var formattedAddress = LocationHelper.FormatAddress(
                    addressLine,
                    locality,
                    region,
                    postcode,
                    country);

                return await GeocodeAddressAsync(formattedAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding address components");
                return new GeocodingResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to geocode address components: {ex.Message}"
                };
            }
        }

        public async Task<string> GetTimezoneForPointAsync(Point point)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            // Create cache key from coordinates (round to 3 decimal places for reasonable cache hits)
            var cacheKey = $"{Math.Round(point.X, 3)},{Math.Round(point.Y, 3)}";

            // Check cache first
            if (_timezoneCache.TryGetValue(cacheKey, out var cachedTimezone))
            {
                return cachedTimezone;
            }

            try
            {
                _logger.LogInformation("Getting timezone for location: ({Longitude}, {Latitude})", point.X, point.Y);

                // Create GeoPosition for Azure Maps
                var position = new GeoPosition(point.Y, point.X);

                // Call Azure Maps to get timezone info
                var options = new GetTimeZoneOptions();
                var response = await Task.FromResult(_timeZoneClient.GetTimeZoneByCoordinates(position, options));

                if (response?.Value == null || response.Value.TimeZones.Count == 0)
                {
                    _logger.LogWarning("No timezone found for location: ({Longitude}, {Latitude})", point.X, point.Y);
                    // Fall back to UTC
                    return "Etc/UTC";
                }

                var timezone = response.Value.TimeZones[0].Id;

                // Cache the result
                _timezoneCache[cacheKey] = timezone;

                return timezone;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting timezone for location: ({Longitude}, {Latitude})", point.X, point.Y);
                // Fall back to UTC
                return "Etc/UTC";
            }
        }

        /// <inheritdoc />
        public async Task<LocalDateTime> ConvertToLocalTimeAsync(Instant instant, Point point)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            try
            {
                // Get timezone for the location
                var timezoneId = await GetTimezoneForPointAsync(point);

                // Get DateTimeZone from IANA ID
                var timeZone = _dateTimeZoneProvider.GetZoneOrNull(timezoneId) ?? DateTimeZone.Utc;

                // Convert UTC instant to local date and time
                var zonedDateTime = instant.InZone(timeZone);
                return zonedDateTime.LocalDateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting time for location: ({Longitude}, {Latitude})", point.X, point.Y);
                // Fall back to UTC
                return instant.InUtc().LocalDateTime;
            }
        }

        /// <inheritdoc />
        public async Task<string> StandardizeAddressAsync(string address)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                {
                    return string.Empty;
                }

                _logger.LogInformation("Standardizing address using Azure Maps: {Address}", address);

                // Use Azure Maps to standardize the address through geocoding and formatting
                var geocodingResult = await GeocodeAddressAsync(address);

                if (!geocodingResult.Success || geocodingResult.Address == null)
                {
                    return address; // Return original if standardization fails
                }

                // Use the formatted address from the geocoding result
                return geocodingResult.Address.FormattedAddress ?? address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error standardizing address: {Address}", address);
                return address; // Return original address if standardization fails
            }
        }
    }
}
