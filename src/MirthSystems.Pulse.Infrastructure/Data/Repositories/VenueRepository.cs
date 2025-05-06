namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;

    using NetTopologySuite.Geometries;

    /// <summary>
    /// Repository for managing venue entities in the database.
    /// </summary>
    /// <remarks>
    /// <para>This repository extends the base repository with venue-specific query methods.</para>
    /// <para>It handles venue-related data access operations including:</para>
    /// <para>- Retrieving venues with related data</para>
    /// <para>- Filtering venues by location</para>
    /// <para>- Implementing soft delete behavior for venues</para>
    /// </remarks>
    public class VenueRepository : Repository<Venue>, IVenueRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VenueRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks>
        /// Passes the database context to the base repository constructor.
        /// </remarks>
        public VenueRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all non-deleted venues.
        /// </summary>
        /// <returns>An IQueryable of non-deleted venues.</returns>
        /// <remarks>
        /// <para>This override implements the soft delete pattern for venues.</para>
        /// <para>Only venues where IsDeleted is false are returned.</para>
        /// </remarks>
        public override IQueryable<Venue> GetAll()
        {
            return _dbSet.Where(v => !v.IsDeleted);
        }

        /// <summary>
        /// Gets a venue by ID with all its related data.
        /// </summary>
        /// <param name="id">The primary key of the venue.</param>
        /// <returns>The venue with its address and business hours if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method eager loads the following related entities:</para>
        /// <para>- Address: Location information for the venue</para>
        /// <para>- BusinessHours: Operating schedules for the venue</para>
        /// <para>Only non-deleted venues are returned.</para>
        /// </remarks>
        public async Task<Venue?> GetVenueWithDetailsAsync(long id)
        {
            return await _context.Venues
                .Include(v => v.Address)
                .Include(v => v.BusinessHours)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        /// <summary>
        /// Gets a paged list of venues.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A tuple containing the list of venues for the requested page and the total count of venues.</returns>
        /// <remarks>
        /// <para>This method implements server-side paging for efficient data retrieval.</para>
        /// <para>Venues are ordered by creation date (newest first) and include their address information.</para>
        /// <para>Only non-deleted venues are included in the results.</para>
        /// </remarks>
        public async Task<(List<Venue> venues, int totalCount)> GetPagedVenuesAsync(int page, int pageSize)
        {
            var query = _context.Venues
                .Include(v => v.Address)
                .Where(v => !v.IsDeleted)
                .OrderByDescending(v => v.CreatedAt);

            var totalCount = await query.CountAsync();

            var venues = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (venues, totalCount);
        }

        /// <summary>
        /// Finds venues near a specific geographic location within a given distance.
        /// </summary>
        /// <param name="location">The geographic point to search around.</param>
        /// <param name="distanceInMeters">The search radius in meters.</param>
        /// <returns>A list of venues within the specified distance of the location.</returns>
        /// <remarks>
        /// <para>This method performs a spatial query using PostGIS capabilities.</para>
        /// <para>Results are ordered by distance from the specified location (closest first).</para>
        /// <para>Each venue includes its address information.</para>
        /// <para>Only non-deleted venues are included in the results.</para>
        /// </remarks>
        public async Task<List<Venue>> FindVenuesNearLocationAsync(Point location, double distanceInMeters)
        {
            return await _context.Venues
                .Include(v => v.Address)
                .Where(v => !v.IsDeleted &&
                           v.Address.Location.Distance(location) <= distanceInMeters)
                .OrderBy(v => v.Address.Location.Distance(location))
                .ToListAsync();
        }
    }
}
