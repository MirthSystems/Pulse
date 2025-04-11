namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    using Pulse.Core.Models;
    using Pulse.Core.Models.Entities;
    using NodaTime;

    public interface IVenueRepository : IRepository<Venue, long>
    {
        /// <summary>
        /// Finds venues within the specified radius of a location
        /// </summary>
        /// <param name="location">Search location point</param>
        /// <param name="radiusMiles">Radius in miles</param>
        /// <returns>List of venues with distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesNearbyAsync(Point location, double radiusMiles);

        /// <summary>
        /// Finds venues with active specials within the specified radius of a location
        /// </summary>
        /// <param name="location">Search location point</param>
        /// <param name="radiusMiles">Radius in miles</param>
        /// <param name="currentInstant">Current time to check for active specials</param>
        /// <returns>List of venues with active specials and distance information</returns>
        Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearbyAsync(
            Point location,
            double radiusMiles,
            Instant currentInstant);

        /// <summary>
        /// Gets a venue with its venue type
        /// </summary>
        Task<Venue?> GetWithVenueTypeAsync(long id);

        /// <summary>
        /// Gets a venue with its business hours
        /// </summary>
        Task<Venue?> GetWithBusinessHoursAsync(long id);

        /// <summary>
        /// Gets a venue with its specials
        /// </summary>
        Task<Venue?> GetWithSpecialsAsync(long id);

        /// <summary>
        /// Gets a venue with all related data
        /// </summary>
        Task<Venue?> GetWithAllDataAsync(long id);
    }
}
