namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for Venue-specific operations
    /// </summary>
    public interface IVenueRepository : IRepository<Venue, long>
    {
        /// <summary>
        /// Finds venues within a specified distance of a location
        /// </summary>
        /// <param name="location">Geographic point location</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues within the radius</returns>
        Task<IEnumerable<Venue>> FindNearbyAsync(Point location, double radiusMiles);

        /// <summary>
        /// Finds venues within a specified distance of a location that have active specials
        /// </summary>
        /// <param name="location">Geographic point location</param>
        /// <param name="radiusMiles">Search radius in miles</param>
        /// <returns>Collection of venues with active specials</returns>
        Task<IEnumerable<Venue>> FindNearbyWithActiveSpecialsAsync(Point location, double radiusMiles);

        /// <summary>
        /// Gets a venue with its related venue type
        /// </summary>
        /// <param name="id">Venue ID</param>
        /// <returns>Venue with venue type if found, null otherwise</returns>
        Task<Venue?> GetWithVenueTypeAsync(long id);

        /// <summary>
        /// Gets a venue with its business hours
        /// </summary>
        /// <param name="id">Venue ID</param>
        /// <returns>Venue with business hours if found, null otherwise</returns>
        Task<Venue?> GetWithBusinessHoursAsync(long id);

        /// <summary>
        /// Gets a venue with its specials
        /// </summary>
        /// <param name="id">Venue ID</param>
        /// <returns>Venue with specials if found, null otherwise</returns>
        Task<Venue?> GetWithSpecialsAsync(long id);

        /// <summary>
        /// Gets a venue with all related data (type, hours, specials)
        /// </summary>
        /// <param name="id">Venue ID</param>
        /// <returns>Venue with all related data if found, null otherwise</returns>
        Task<Venue?> GetWithAllDataAsync(long id);
    }
}
