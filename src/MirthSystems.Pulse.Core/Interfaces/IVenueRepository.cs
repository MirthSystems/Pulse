namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Models;

    using NetTopologySuite.Geometries;

    /// <summary>
    /// Repository interface for venue entities, extending the base repository with venue-specific operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines venue-specific data access operations beyond the basic CRUD operations.</para>
    /// <para>It provides methods for retrieving venues with related data, pagination, and location-based queries.</para>
    /// <para>Implementations handle the actual data access logic and database interactions.</para>
    /// </remarks>
    public interface IVenueRepository : IRepository<Venue>
    {
        /// <summary>
        /// Gets a venue by ID with all its related data including address and operating schedules.
        /// </summary>
        /// <param name="id">The primary key of the venue to retrieve.</param>
        /// <returns>The venue with its related data if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method implements eager loading of related entities to reduce database round trips.</para>
        /// <para>Related entities loaded include:</para>
        /// <para>- Address: The venue's physical location</para>
        /// <para>- BusinessHours: The venue's operating schedules for each day of the week</para>
        /// </remarks>
        Task<Venue?> GetVenueWithDetailsAsync(long id);

        /// <summary>
        /// Gets a paged list of venues for efficient retrieval of large result sets.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of venues per page.</param>
        /// <returns>A tuple containing the list of venues for the requested page and the total count of venues.</returns>
        /// <remarks>
        /// <para>This method implements server-side pagination to improve performance and resource utilization.</para>
        /// <para>The venues are ordered by creation date (newest first).</para>
        /// <para>Each venue includes its basic information and address for display purposes.</para>
        /// <para>Soft-deleted venues are excluded from the results.</para>
        /// </remarks>
        Task<PagedList<Venue>> GetPagedVenuesAsync(int page, int pageSize);

        /// <summary>
        /// Finds venues near a specific geographic location within a given distance.
        /// </summary>
        /// <param name="location">The geographic point to search around.</param>
        /// <param name="distanceInMeters">The search radius in meters.</param>
        /// <returns>A list of venues within the specified distance of the location.</returns>
        /// <remarks>
        /// <para>This method performs spatial queries using the database's geographic capabilities.</para>
        /// <para>The search uses geodesic distance calculation (taking earth's curvature into account).</para>
        /// <para>Results are ordered by distance from the specified location (closest first).</para>
        /// <para>Each venue includes its address information.</para>
        /// <para>Soft-deleted venues are excluded from the results.</para>
        /// </remarks>
        Task<List<Venue>> FindVenuesNearLocationAsync(Point location, double distanceInMeters);
    }
}
