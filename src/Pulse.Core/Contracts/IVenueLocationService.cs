namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    using NodaTime;

    using Pulse.Core.Models;
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Service for handling venue location operations
    /// </summary>
    public interface IVenueLocationService
    {
        /// <summary>
        /// Geocodes and populates location data for a venue
        /// </summary>
        /// <param name="venue">Venue to update with location data</param>
        /// <returns>True if geocoding was successful, false otherwise</returns>
        Task<bool> GeocodeVenueAsync(Venue venue);

        /// <summary>
        /// Finds venues near an address within a specified radius
        /// </summary>
        /// <param name="address">Address to search near</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesNearAddressAsync(string address, double radiusMiles);

        /// <summary>
        /// Finds venues near a point within a specified radius
        /// </summary>
        /// <param name="latitude">Latitude of the search point</param>
        /// <param name="longitude">Longitude of the search point</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesNearPointAsync(double latitude, double longitude, double radiusMiles);

        /// <summary>
        /// Finds venues with active specials near an address within a specified radius
        /// </summary>
        /// <param name="address">Address to search near</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with active specials and distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearAddressAsync(string address, double radiusMiles);

        /// <summary>
        /// Finds venues with active specials near a point within a specified radius
        /// </summary>
        /// <param name="latitude">Latitude of the search point</param>
        /// <param name="longitude">Longitude of the search point</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with active specials and distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearPointAsync(double latitude, double longitude, double radiusMiles);

        /// <summary>
        /// Gets the current local time at a geographic point
        /// </summary>
        /// <param name="point">Geographic point</param>
        /// <returns>Local date and time at the specified location</returns>
        Task<LocalDateTime> GetLocalTimeAtPointAsync(Point point);
    }
}
