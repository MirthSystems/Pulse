namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Enums;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Requests;
    using NetTopologySuite.Geometries;
    using NodaTime;

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
        Task<Venue?> GetVenueWithDetailsAsync(long id);

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
        Task<PagedList<Venue>> GetPagedVenuesAsync(
            int page,
            int pageSize,
            Point? location = null,
            double? distanceInMeters = null,
            string? searchTerm = null,
            DayOfWeek? openOnDay = null,
            string? openAtTime = null,
            bool? hasActiveSpecials = null,
            int? specialTypeId = null);

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
        
        /// <summary>
        /// Determines if a venue is open at a specific day and time.
        /// </summary>
        /// <param name="venueId">The ID of the venue to check.</param>
        /// <param name="dayOfWeek">The day of week to check.</param>
        /// <param name="time">The time to check in format "HH:mm".</param>
        /// <returns>True if the venue is open at the specified day and time; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method checks the operating schedule of a venue for the specified day.</para>
        /// <para>It handles cases where venues close after midnight by checking if the time falls between opening and closing hours.</para>
        /// </remarks>
        Task<bool> IsVenueOpenAsync(long venueId, DayOfWeek dayOfWeek, string time);
        
        /// <summary>
        /// Gets venues that have at least one active special at the current time.
        /// </summary>
        /// <param name="specialTypeId">Optional filter for specific type of special.</param>
        /// <returns>A list of venue IDs with active specials.</returns>
        /// <remarks>
        /// <para>This method checks current and recurring specials to determine which are active.</para>
        /// <para>It accounts for start/end dates, recurring patterns, and expiration dates.</para>
        /// </remarks>
        Task<List<long>> GetVenueIdsWithActiveSpecialsAsync(int? specialTypeId = null);

        /// <summary>
        /// Gets venues with their running specials for a specific search area and time.
        /// </summary>
        /// <param name="searchPoint">The center point of the search area.</param>
        /// <param name="distanceInMeters">The radius to search around the point in meters.</param>
        /// <param name="searchAt">The instant in time to check for running specials.</param>
        /// <param name="searchTerm">Optional text to filter venues and specials.</param>
        /// <param name="specialType">Optional type to filter specials.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <returns>Paged list of venues with their matching specials.</returns>
        Task<PagedList<(Venue Venue, List<Special> Specials)>> GetVenuesWithRunningSpecialsAsync(
            Point searchPoint,
            double distanceInMeters,
            Instant searchAt,
            string? searchTerm = null,
            SpecialTypes? specialType = null,
            int page = 1,
            int pageSize = 20);
    }
}
