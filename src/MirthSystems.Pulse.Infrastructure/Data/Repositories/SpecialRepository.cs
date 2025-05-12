namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Cronos;

    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Enums;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Utilities;

    using NetTopologySuite.Geometries;

    using NodaTime;

    /// <summary>
    /// Repository for managing special promotion entities in the database.
    /// </summary>
    /// <remarks>
    /// <para>This repository extends the base repository with special-specific query methods.</para>
    /// <para>It handles special-related data access operations including:</para>
    /// <para>- Retrieving specials with their associated venues</para>
    /// <para>- Filtering specials by location, type, and status</para>
    /// <para>- Calculating whether specials are currently active based on their scheduling information</para>
    /// </remarks>
    public class SpecialRepository : Repository<Special>, ISpecialRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks>
        /// Passes the database context to the base repository constructor.
        /// </remarks>
        public SpecialRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all non-deleted specials.
        /// </summary>
        /// <returns>An IQueryable of non-deleted specials.</returns>
        /// <remarks>
        /// <para>This override implements the soft delete pattern for specials.</para>
        /// <para>Only specials where IsDeleted is false are returned.</para>
        /// </remarks>
        public override IQueryable<Special> GetAll()
        {
            return _dbSet.Where(s => !s.IsDeleted);
        }

        /// <summary>
        /// Gets a special by ID with its associated venue and address information.
        /// </summary>
        /// <param name="id">The primary key of the special.</param>
        /// <returns>The special with its venue and venue address if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method eager loads the following related entities:</para>
        /// <para>- Venue: The venue offering this special</para>
        /// <para>- Venue.Address: The location of the venue</para>
        /// <para>Only non-deleted specials are returned.</para>
        /// </remarks>
        public async Task<Special?> GetSpecialWithVenueAsync(long id)
        {
            return await _context.Specials
                .Include(s => s.Venue)
                .ThenInclude(v => v!.Address)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        /// <summary>
        /// Gets a paged list of specials with optional filtering.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="location">Optional geographic point to filter by proximity.</param>
        /// <param name="distanceInMeters">Optional search radius in meters when location is specified.</param>
        /// <param name="searchTerm">Optional text to search in special content and venue names.</param>
        /// <param name="type">Optional special type to filter by.</param>
        /// <param name="includeExpired">Whether to include specials with passed expiration dates.</param>
        /// <returns>A tuple containing the list of specials for the requested page and the total count of specials matching the filters.</returns>
        /// <remarks>
        /// <para>This method implements server-side paging with multiple filtering options.</para>
        /// <para>Filtering capabilities include:</para>
        /// <para>- Location-based filtering (specials at venues within a specific distance of a point)</para>
        /// <para>- Text search in special descriptions and venue names</para>
        /// <para>- Type filtering (food, drink, entertainment)</para>
        /// <para>- Expiration filtering to exclude expired specials</para>
        /// <para>Specials are ordered by creation date (newest first) and venue name.</para>
        /// <para>Only non-deleted specials from non-deleted venues are included in the results.</para>
        /// <para>Text search is case-insensitive and matches partial content in special descriptions or venue names.</para>
        /// </remarks>
        public async Task<PagedList<Special>> GetPagedSpecialsAsync(
            int page,
            int pageSize,
            Point? location = null,
            double? distanceInMeters = null,
            string? searchTerm = null,
            SpecialTypes? type = null,
            bool includeExpired = false)
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var today = LocalDate.FromDateTime(DateTime.Today);

            var query = _context.Specials
                .Include(s => s.Venue)
                .ThenInclude(v => v!.Address)
                .Where(s => !s.IsDeleted && !s.Venue!.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(s => s.Content.ToLower().Contains(searchTerm) ||
                                        s.Venue!.Name.ToLower().Contains(searchTerm));
            }

            if (type.HasValue)
            {
                query = query.Where(s => s.Type == type.Value);
            }

            if (location != null && distanceInMeters.HasValue)
            {
                query = query.Where(s => s.Venue!.Address.Location.Distance(location) <= distanceInMeters.Value);
            }

            if (!includeExpired)
            {
                query = query.Where(s => s.ExpirationDate == null || s.ExpirationDate >= today);
            }

            query = query.OrderByDescending(s => s.CreatedAt)
                         .ThenBy(s => s.Venue!.Name);

            var totalCount = await query.CountAsync();

            return await PagedList<Special>.CreateAsync(query, page, pageSize);
        }

        /// <summary>
        /// Gets all specials for a specific venue.
        /// </summary>
        /// <param name="venueId">The primary key of the venue.</param>
        /// <returns>A list of specials associated with the venue.</returns>
        /// <remarks>
        /// <para>This method retrieves all non-deleted specials for a specific venue.</para>
        /// <para>Results are ordered by start date and then by start time.</para>
        /// <para>This ordering helps display specials in a logical sequence for users.</para>
        /// </remarks>
        public async Task<List<Special>> GetSpecialsByVenueIdAsync(long venueId)
        {
            return await _context.Specials
                .Where(s => s.VenueId == venueId && !s.IsDeleted)
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// Determines if a special is currently active based on its schedule.
        /// </summary>
        /// <param name="specialId">The primary key of the special.</param>
        /// <param name="referenceTime">The instant in time to check against (typically the current time).</param>
        /// <returns>True if the special is currently active; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs complex time-based calculations to determine if a special is active.</para>
        /// <para>It handles one-time specials, recurring specials with CRON schedules, and various edge cases.</para>
        /// <para>The logic accounts for:</para>
        /// <para>- Start dates and times</para>
        /// <para>- End times (including those that cross midnight)</para>
        /// <para>- Expiration dates</para>
        /// <para>- CRON-based recurrence patterns</para>
        /// </remarks>
        public async Task<bool> IsSpecialCurrentlyActiveAsync(long specialId, Instant referenceTime)
        {
            var special = await _context.Specials
                .FirstOrDefaultAsync(s => s.Id == specialId && !s.IsDeleted);

            if (special == null)
            {
                return false;
            }

            return SpecialActivityUtility.IsSpecialActive(special, referenceTime);
        }
    }
}
