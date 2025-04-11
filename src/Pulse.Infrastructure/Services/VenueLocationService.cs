﻿namespace Pulse.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    using Pulse.Core.Models;
    using NetTopologySuite.Geometries;
    using Pulse.Core.Utilities;

    /// <summary>
    /// Service for handling venue location operations
    /// </summary>
    public class VenueLocationService : IVenueLocationService
    {
        private readonly ILogger<VenueLocationService> _logger;
        private readonly IVenueRepository _venueRepository;
        private readonly ILocationService _locationService;
        private readonly IClock _clock;

        public VenueLocationService(
            ILogger<VenueLocationService> logger,
            IVenueRepository venueRepository,
            ILocationService locationService,
            IClock clock)
        {
            _logger = logger;
            _venueRepository = venueRepository;
            _locationService = locationService;
            _clock = clock;
        }

        /// <inheritdoc />
        public async Task<bool> GeocodeVenueAsync(Venue venue, CancellationToken cancellationToken = default)
        {
            try
            {
                if (venue == null)
                {
                    throw new ArgumentNullException(nameof(venue));
                }

                _logger.LogInformation("Geocoding venue: {VenueName}", venue.Name);

                // For geocoding, primarily use addressLine1 as that's the main address component
                var geocodingResult = await _locationService.GeocodeAddressComponentsAsync(
                    venue.AddressLine1,
                    venue.Locality,
                    venue.Region,
                    venue.Postcode,
                    venue.Country,
                    cancellationToken);

                if (!geocodingResult.Success || geocodingResult.Point == null)
                {
                    _logger.LogWarning(
                        "Failed to geocode venue {VenueName}: {ErrorMessage}",
                        venue.Name,
                        geocodingResult.ErrorMessage);
                    return false;
                }

                // Update venue with location data
                venue.Location = geocodingResult.Point;

                // If we have address details from Azure Maps, update the venue
                if (geocodingResult.Address != null)
                {
                    venue.UpdateFromMapsAddress(geocodingResult.Address);
                }

                _logger.LogInformation(
                    "Successfully geocoded venue {VenueName} to ({Longitude}, {Latitude})",
                    venue.Name,
                    geocodingResult.Point.X,
                    geocodingResult.Point.Y);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding venue {VenueName}", venue?.Name);
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesNearAddressAsync(
            string address,
            double radiusMiles,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Address cannot be empty", nameof(address));
                }

                // Geocode the address to get coordinates
                var geocodingResult = await _locationService.GeocodeAddressAsync(address, cancellationToken);

                if (!geocodingResult.Success || geocodingResult.Point == null)
                {
                    _logger.LogWarning(
                        "Failed to geocode address for venue search: {ErrorMessage}",
                        geocodingResult.ErrorMessage);
                    return Enumerable.Empty<VenueWithDistance>();
                }

                // Use the coordinates to find nearby venues
                return await FindVenuesNearPointAsync(
                    geocodingResult.Point.Y,  // Latitude
                    geocodingResult.Point.X,  // Longitude
                    radiusMiles,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding venues near address: {Address}", address);
                return Enumerable.Empty<VenueWithDistance>();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesNearPointAsync(
            double latitude,
            double longitude,
            double radiusMiles,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Create a point from the coordinates
                var searchPoint = new Point(longitude, latitude) { SRID = 4326 };

                _logger.LogInformation(
                    "Finding venues near point ({Longitude}, {Latitude}) within {Radius} miles",
                    longitude,
                    latitude,
                    radiusMiles);

                // Get current time for the location
                var localTime = await GetLocalTimeAtPointAsync(searchPoint, cancellationToken);

                // Get venues within the radius
                var venues = await _venueRepository.FindNearbyAsync(searchPoint, radiusMiles);

                // Calculate exact distances and create DTOs
                return venues
                    .Select(venue => new VenueWithDistance
                    {
                        Venue = venue,
                        DistanceMiles = venue.Location != null
                            ? LocationHelper.CalculateDistanceInMiles(searchPoint, venue.Location)
                            : double.MaxValue,
                        SearchPoint = searchPoint,
                        LocalTime = localTime
                    })
                    .OrderBy(v => v.DistanceMiles)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error finding venues near point ({Longitude}, {Latitude})",
                    longitude,
                    latitude);
                return Enumerable.Empty<VenueWithDistance>();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearAddressAsync(
            string address,
            double radiusMiles,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Address cannot be empty", nameof(address));
                }

                // Geocode the address to get coordinates
                var geocodingResult = await _locationService.GeocodeAddressAsync(address, cancellationToken);

                if (!geocodingResult.Success || geocodingResult.Point == null)
                {
                    _logger.LogWarning(
                        "Failed to geocode address for venue search: {ErrorMessage}",
                        geocodingResult.ErrorMessage);
                    return Enumerable.Empty<VenueWithDistance>();
                }

                // Use the coordinates to find nearby venues with active specials
                return await FindVenuesWithActiveSpecialsNearPointAsync(
                    geocodingResult.Point.Y,  // Latitude
                    geocodingResult.Point.X,  // Longitude
                    radiusMiles,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding venues with active specials near address: {Address}", address);
                return Enumerable.Empty<VenueWithDistance>();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearPointAsync(
            double latitude,
            double longitude,
            double radiusMiles,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Create a point from the coordinates
                var searchPoint = new Point(longitude, latitude) { SRID = 4326 };

                _logger.LogInformation(
                    "Finding venues with active specials near point ({Longitude}, {Latitude}) within {Radius} miles",
                    longitude,
                    latitude,
                    radiusMiles);

                // Get current time at the search location for evaluating "active" specials
                var now = _clock.GetCurrentInstant();
                var localTime = await _locationService.ConvertToLocalTimeAsync(now, searchPoint, cancellationToken);

                // Get venues with active specials within the radius
                var venues = await _venueRepository.FindNearbyWithActiveSpecialsAsync(searchPoint, radiusMiles);

                // Calculate exact distances and create DTOs
                return venues
                    .Select(venue => new VenueWithDistance
                    {
                        Venue = venue,
                        DistanceMiles = venue.Location != null
                            ? LocationHelper.CalculateDistanceInMiles(searchPoint, venue.Location)
                            : double.MaxValue,
                        SearchPoint = searchPoint,
                        LocalTime = localTime
                    })
                    .OrderBy(v => v.DistanceMiles)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error finding venues with active specials near point ({Longitude}, {Latitude})",
                    longitude,
                    latitude);
                return Enumerable.Empty<VenueWithDistance>();
            }
        }

        public async Task<LocalDateTime> GetLocalTimeAtPointAsync(Point point, CancellationToken cancellationToken = default)
        {
            try
            {
                if (point == null)
                {
                    throw new ArgumentNullException(nameof(point));
                }

                // Get current UTC time
                var currentInstant = _clock.GetCurrentInstant();

                // Convert to location's local time
                return await _locationService.ConvertToLocalTimeAsync(currentInstant, point, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting local time for point: ({Longitude}, {Latitude})", point?.X, point?.Y);
                return _clock.GetCurrentInstant().InUtc().LocalDateTime;
            }
        }
    }
}
