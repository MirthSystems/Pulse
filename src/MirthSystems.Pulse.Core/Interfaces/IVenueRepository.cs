namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;

    using NetTopologySuite.Geometries;

    public interface IVenueRepository : IRepository<Venue>
    {
        /// <summary>
        /// Gets a venue by ID with all related data (address, operating schedules)
        /// </summary>
        Task<Venue?> GetVenueWithDetailsAsync(long id);

        /// <summary>
        /// Gets a paged list of venues
        /// </summary>
        Task<(List<Venue> venues, int totalCount)> GetPagedVenuesAsync(int page, int pageSize);

        /// <summary>
        /// Finds venues near a specific point within a given distance (in meters)
        /// </summary>
        Task<List<Venue>> FindVenuesNearLocationAsync(Point location, double distanceInMeters);
    }
}
