namespace Pulse.Infrastructure.Services
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocationService _locationService;
        private readonly IClock _clock;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenueLocationService"/> class.
        /// </summary>
        /// <param name="logger">Logger for diagnostic information</param>
        /// <param name="unitOfWork">Unit of work for coordinated database operations</param>
        /// <param name="locationService">Service for location operations</param>
        /// <param name="clock">Clock for current time</param>
        public VenueLocationService(
            ILogger<VenueLocationService> logger,
            IUnitOfWork unitOfWork,
            ILocationService locationService,
            IClock clock)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _locationService = locationService;
            _clock = clock;
        }

        public async Task<bool> GeocodeVenueAsync(Venue venue)
        {
            try
            {
                if (venue == null)
                    throw new ArgumentNullException(nameof(venue));

                _logger.LogInformation("Geocoding venue: {VenueName}", venue.Name);

                var geocodingResult = await _locationService.GeocodeAddressComponentsAsync(
                    venue.AddressLine1,
                    venue.Locality,
                    venue.Region,
                    venue.Postcode,
                    venue.Country);

                if (!geocodingResult.Success || geocodingResult.Point == null)
                {
                    _logger.LogWarning(
                        "Failed to geocode venue {VenueName}: {ErrorMessage}",
                        venue.Name,
                        geocodingResult.ErrorMessage);
                    return false;
                }

                venue.Location = geocodingResult.Point;

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

        /// <summary>
        /// Finds venues near a specified address
        /// </summary>
        /// <param name="address">The address to search from</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with distance information</returns>
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesNearAddressAsync(
            string address,
            double radiusMiles)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                    throw new ArgumentException("Address cannot be empty", nameof(address));

                var geocodingResult = await _locationService.GeocodeAddressAsync(address);

                if (!geocodingResult.Success || geocodingResult.Point == null)
                {
                    _logger.LogWarning(
                        "Failed to geocode address for venue search: {ErrorMessage}",
                        geocodingResult.ErrorMessage);
                    return Enumerable.Empty<VenueWithDistance>();
                }

                return await FindVenuesNearPointAsync(
                    geocodingResult.Point.Y,
                    geocodingResult.Point.X,
                    radiusMiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding venues near address: {Address}", address);
                return Enumerable.Empty<VenueWithDistance>();
            }
        }

        /// <summary>
        /// Finds venues near a specified geographic point
        /// </summary>
        /// <param name="latitude">Latitude of the search point</param>
        /// <param name="longitude">Longitude of the search point</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with distance information</returns>
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesNearPointAsync(
            double latitude,
            double longitude,
            double radiusMiles)
        {
            try
            {
                var searchPoint = new Point(longitude, latitude) { SRID = 4326 };

                _logger.LogInformation(
                    "Finding venues near point ({Longitude}, {Latitude}) within {Radius} miles",
                    longitude,
                    latitude,
                    radiusMiles);

                var localTime = await GetLocalTimeAtPointAsync(searchPoint);

                var venues = await _unitOfWork.Venues.FindVenuesNearbyAsync(searchPoint, radiusMiles);

                foreach (var venue in venues)
                {
                    venue.LocalTime = localTime;
                }

                return venues;
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

        /// <summary>
        /// Finds venues with active specials near a specified address
        /// </summary>
        /// <param name="address">The address to search from</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with active specials and distance information</returns>
        public async Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearAddressAsync(
            string address,
            double radiusMiles)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                    throw new ArgumentException("Address cannot be empty", nameof(address));

                var geocodingResult = await _locationService.GeocodeAddressAsync(address);

                if (!geocodingResult.Success || geocodingResult.Point == null)
                {
                    _logger.LogWarning(
                        "Failed to geocode address for venue search: {ErrorMessage}",
                        geocodingResult.ErrorMessage);
                    return Enumerable.Empty<VenueWithDistance>();
                }

                return await FindVenuesWithActiveSpecialsNearPointAsync(
                    geocodingResult.Point.Y,
                    geocodingResult.Point.X,
                    radiusMiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding venues with active specials near address: {Address}", address);
                return Enumerable.Empty<VenueWithDistance>();
            }
        }

        public async Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearPointAsync(
            double latitude,
            double longitude,
            double radiusMiles)
        {
            try
            {
                var searchPoint = new Point(longitude, latitude) { SRID = 4326 };

                _logger.LogInformation(
                    "Finding venues with active specials near point ({Longitude}, {Latitude}) within {Radius} miles",
                    longitude,
                    latitude,
                    radiusMiles);

                var now = _clock.GetCurrentInstant();
                var localTime = await _locationService.ConvertToLocalTimeAsync(now, searchPoint);

                var venues = await _unitOfWork.Venues.FindVenuesWithActiveSpecialsNearbyAsync(
                    searchPoint,
                    radiusMiles,
                    now);

                foreach (var venue in venues)
                {
                    venue.LocalTime = localTime;
                }

                return venues;
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

        /// <summary>
        /// Gets the local time at a specific geographic point.
        /// </summary>
        /// <param name="point">Geographic point</param>
        /// <returns>Local date and time at the specified location</returns>
        public async Task<LocalDateTime> GetLocalTimeAtPointAsync(Point point)
        {
            try
            {
                if (point == null)
                    throw new ArgumentNullException(nameof(point));

                var currentInstant = _clock.GetCurrentInstant();

                return await _locationService.ConvertToLocalTimeAsync(currentInstant, point);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting local time for point: ({Longitude}, {Latitude})", point?.X, point?.Y);
                return _clock.GetCurrentInstant().InUtc().LocalDateTime;
            }
        }
    }
}
