namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    using NodaTime;

    using Pulse.Core.Models;
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Service for venue location operations
    /// </summary>
    public interface IVenueLocationService
    {
        /// <summary>
        /// Geocodes a venue's address to obtain geographic coordinates
        /// </summary>
        /// <param name="venue">The venue to geocode</param>
        /// <returns>True if geocoding succeeds, false otherwise</returns>
        Task<bool> GeocodeVenueAsync(Venue venue);

        /// <summary>
        /// Finds venues near a specified address
        /// </summary>
        /// <param name="address">The address to search from</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesNearAddressAsync(
            string address,
            double radiusMiles);

        /// <summary>
        /// Finds venues near a specified geographic point
        /// </summary>
        /// <param name="latitude">Latitude of the search point</param>
        /// <param name="longitude">Longitude of the search point</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesNearPointAsync(
            double latitude,
            double longitude,
            double radiusMiles);

        /// <summary>
        /// Finds venues with active specials near a specified address
        /// </summary>
        /// <param name="address">The address to search from</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with active specials and distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearAddressAsync(
            string address,
            double radiusMiles);

        /// <summary>
        /// Finds venues with active specials near a specified geographic point
        /// </summary>
        /// <param name="latitude">Latitude of the search point</param>
        /// <param name="longitude">Longitude of the search point</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with active specials and distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearPointAsync(
            double latitude,
            double longitude,
            double radiusMiles);

        /// <summary>
        /// Gets the local time at a specific geographic point
        /// </summary>
        /// <param name="point">Geographic point</param>
        /// <returns>Local date and time at the specified location</returns>
        Task<LocalDateTime> GetLocalTimeAtPointAsync(Point point);
    }
}
