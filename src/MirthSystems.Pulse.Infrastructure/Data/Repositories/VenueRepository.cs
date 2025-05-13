namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using NetTopologySuite.Geometries;

    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Enums;
    using MirthSystems.Pulse.Core.Utilities;
    using Cronos;

    /// <summary>
    /// Repository implementation for venue-related data access operations.
    /// </summary>
    /// <remarks>
    /// <para>This class handles all database interactions related to venue entities.</para>
    /// <para>Extends the base Repository class with venue-specific query methods.</para>
    /// </remarks>
    public class VenueRepository : Repository<Venue>, IVenueRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VenueRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context for entity operations.</param>
        public VenueRepository(ApplicationDbContext dbContext) : base(dbContext)
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
        /// Gets a paged list of venues with optional filtering.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="location">Optional geographic point to filter by proximity.</param>
        /// <param name="distanceInMeters">Optional search radius in meters when location is specified.</param>
        /// <param name="searchTerm">Optional text to search in venue name and description.</param>
        /// <param name="openOnDay">Optional day of week to filter venues that are open on that day.</param>
        /// <param name="openAtTime">Optional time to filter venues that are open at that specific time (format: "HH:mm").</param>
        /// <param name="hasActiveSpecials">Optional filter to include only venues with active specials.</param>
        /// <param name="specialTypeId">Optional filter for venues with specials of a specific type.</param>
        /// <returns>A paged list of venues matching the filter criteria.</returns>
        /// <remarks>
        /// <para>This method implements server-side pagination with flexible filtering options.</para>
        /// <para>Filtering capabilities include:</para>
        /// <para>- Text search in venue names and descriptions</para>
        /// <para>- Location-based filtering (venues within a specific distance of a point)</para>
        /// <para>- Operating hours filtering (venues open on specific days/times)</para>
        /// <para>- Special availability filtering (venues with active specials)</para>
        /// <para>Results include venue information with address details by default.</para>
        /// <para>Results are ordered by name by default but can be ordered by distance when location is provided.</para>
        /// </remarks>
        public async Task<PagedList<Venue>> GetPagedVenuesAsync(
            int page,
            int pageSize,
            Point? location = null,
            double? distanceInMeters = null,
            string? searchTerm = null,
            DayOfWeek? openOnDay = null,
            string? openAtTime = null,
            bool? hasActiveSpecials = null,
            int? specialTypeId = null)
        {
            // Start with basic query for non-deleted venues
            var query = _context.Venues
                .Where(v => !v.IsDeleted)
                .Include(v => v.Address)
                .AsQueryable();

            // Apply text search filter if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string search = searchTerm.ToLower();
                query = query.Where(v => 
                    v.Name.ToLower().Contains(search) || 
                    (v.Description != null && v.Description.ToLower().Contains(search)));
            }

            // Apply location filter if provided
            if (location != null && distanceInMeters.HasValue)
            {
                query = query.Where(v => v.Address != null && 
                    v.Address.Location.Distance(location) <= distanceInMeters.Value);
                    
                // Order by distance when doing location-based search
                query = query.OrderBy(v => v.Address.Location.Distance(location));
            }
            else
            {
                // Default ordering by name
                query = query.OrderBy(v => v.Name);
            }
            
            // Apply open day filter if provided
            if (openOnDay.HasValue)
            {
                DayOfWeek day = openOnDay.Value;
                
                query = query.Where(v => 
                    v.BusinessHours.Any(h => 
                        h.DayOfWeek == day && !h.IsClosed
                    )
                );
                
                // If time is also specified, further filter venues open at that time
                if (!string.IsNullOrEmpty(openAtTime) && 
                    TimeOnly.TryParse(openAtTime, out TimeOnly requestTime))
                {
                    // Convert TimeOnly to LocalTime for comparison
                    var requestLocalTime = new LocalTime(requestTime.Hour, requestTime.Minute);
                    
                    query = query.Where(v => 
                        v.BusinessHours.Any(h => 
                            h.DayOfWeek == day && !h.IsClosed &&
                            (
                                // Handle normal operating hours (open time before close time)
                                (h.TimeOfOpen <= h.TimeOfClose && 
                                    h.TimeOfOpen <= requestLocalTime && requestLocalTime <= h.TimeOfClose) ||
                                // Handle operating hours spanning midnight (open time after close time)
                                (h.TimeOfOpen > h.TimeOfClose && 
                                    (h.TimeOfOpen <= requestLocalTime || requestLocalTime <= h.TimeOfClose))
                            )
                        )
                    );
                }
            }
            
            // Apply active specials filter if requested
            if (hasActiveSpecials == true)
            {
                // This requires a separate query as it involves complex time calculations
                var venueIdsWithActiveSpecials = await GetVenueIdsWithActiveSpecialsAsync(specialTypeId);
                query = query.Where(v => venueIdsWithActiveSpecials.Contains(v.Id));
            }
            // If only filtering by special type without checking active status
            else if (specialTypeId.HasValue)
            {
                query = query.Where(v => 
                    v.Specials.Any(s => 
                        !s.IsDeleted && 
                        (int)s.Type == specialTypeId.Value
                    )
                );
            }

            return await PagedList<Venue>.CreateAsync(query, page, pageSize);
        }

        /// <summary>
        /// Gets venues that have at least one active special at the current time.
        /// </summary>
        /// <param name="specialTypeId">Optional filter for specific type of special.</param>
        /// <returns>A list of venue IDs with active specials.</returns>
        public async Task<List<long>> GetVenueIdsWithActiveSpecialsAsync(int? specialTypeId = null)
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var today = LocalDate.FromDateTime(now.ToDateTimeUtc());
            var currentTime = LocalTime.FromTimeOnly(TimeOnly.Parse(now.ToDateTimeUtc().ToString("HH:mm:ss")));

            // Build the query for active specials
            var query = _context.Specials
                .Where(s => !s.IsDeleted)
                .Where(s => s.StartDate <= today)
                .Where(s => s.ExpirationDate == null || s.ExpirationDate >= today)
                .Where(s => s.StartTime <= currentTime && (s.EndTime == null || s.EndTime >= currentTime));

            // Filter by special type if provided
            if (specialTypeId.HasValue)
            {
                query = query.Where(s => (int)s.Type == specialTypeId.Value);
            }

            // Get distinct venue IDs
            return await query
                .Select(s => s.VenueId)
                .Distinct()
                .ToListAsync();
        }

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
        public async Task<List<Venue>> FindVenuesNearLocationAsync(Point location, double distanceInMeters)
        {
            return await _context.Venues
                .Where(v => !v.IsDeleted)
                .Include(v => v.Address)
                .Where(v => v.Address != null && v.Address.Location.Distance(location) <= distanceInMeters)
                .OrderBy(v => v.Address.Location.Distance(location))
                .ToListAsync();
        }
        
        /// <summary>
        /// Determines if a venue is open at a specific day and time.
        /// </summary>
        /// <param name="venueId">The ID of the venue to check.</param>
        /// <param name="dayOfWeek">The day of week to check.</param>
        /// <param name="time">The time to check in format "HH:mm".</param>
        /// <returns>True if the venue is open at the specified day and time; otherwise, false.</returns>
        public async Task<bool> IsVenueOpenAsync(long venueId, DayOfWeek dayOfWeek, string time)
        {
            if (!TimeOnly.TryParse(time, out TimeOnly requestTime))
            {
                return false;
            }
            
            // Convert TimeOnly to LocalTime for comparison
            var requestLocalTime = new LocalTime(requestTime.Hour, requestTime.Minute);
            
            var schedule = await _context.OperatingSchedules
                .FirstOrDefaultAsync(h => 
                    h.VenueId == venueId && 
                    h.DayOfWeek == dayOfWeek && 
                    !h.IsClosed);
                
            if (schedule == null)
            {
                return false;
            }
            
            // Handle normal operating hours (open time before close time)
            if (schedule.TimeOfOpen <= schedule.TimeOfClose)
            {
                return schedule.TimeOfOpen <= requestLocalTime && requestLocalTime <= schedule.TimeOfClose;
            }
            
            // Handle operating hours spanning midnight (open time after close time)
            return schedule.TimeOfOpen <= requestLocalTime || requestLocalTime <= schedule.TimeOfClose;
        }

        /// <summary>
        /// Gets venues with their running specials for a specific search area and time.
        /// </summary>
        /// <param name="searchPoint">The center point of the search area.</param>
        /// <param name="distanceInMeters">The radius to search around the point in meters.</param>
        /// <param name="searchAt">The instant in time to check for running specials.</param>
        /// <param name="searchTerm">Optional text to filter specials.</param>
        /// <param name="specialType">Optional type to filter specials.</param>
        /// <returns>list of venues with their matching specials.</returns>
        public async Task<List<(Venue Venue, List<Special> Specials)>> GetVenuesWithRunningSpecialsAsync(
            Point searchPoint,
            double distanceInMeters,
            Instant searchAt,
            string? searchTerm = null,
            SpecialTypes? specialType = null)
        {
            var venueQuery = _context.Venues
                .Where(v => !v.IsDeleted)
                .Include(v => v.Address)
                .Where(v => v.Address != null &&
                           v.Address.Location.Distance(searchPoint) <= distanceInMeters);

            var venues = await venueQuery.ToListAsync();
            if (!venues.Any())
            {
                return new List<(Venue, List<Special>)>();
            }

            var searchDate = LocalDate.FromDateTime(searchAt.ToDateTimeUtc().Date);
            var venueIds = venues.Select(v => v.Id).ToList();

            var specialsQuery = _context.Specials
                .Where(s => !s.IsDeleted && venueIds.Contains(s.VenueId))
                .Where(s => s.StartDate <= searchDate &&
                           (s.ExpirationDate == null || s.ExpirationDate >= searchDate));

            if (specialType.HasValue)
            {
                specialsQuery = specialsQuery.Where(s => s.Type == specialType);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string search = searchTerm.ToLower();
                specialsQuery = specialsQuery.Where(s => s.Content.ToLower().Contains(search));
            }

            var allSpecials = await specialsQuery.ToListAsync();
            if (!allSpecials.Any())
            {
                return new List<(Venue, List<Special>)>();
            }

            var activeSpecials = allSpecials
                .Where(s => SpecialActivityUtility.IsSpecialActive(s, searchAt))
                .ToList();

            var venuesWithSpecials = new List<(Venue Venue, List<Special> Specials)>();

            foreach (var venue in venues)
            {
                var venueActiveSpecials = activeSpecials
                    .Where(s => s.VenueId == venue.Id)
                    .ToList();

                if (venueActiveSpecials.Any())
                {
                    venuesWithSpecials.Add((venue, venueActiveSpecials));
                }
            }

            return venuesWithSpecials.OrderBy(vs => vs.Venue.Name).ToList();
        }
    }
}
