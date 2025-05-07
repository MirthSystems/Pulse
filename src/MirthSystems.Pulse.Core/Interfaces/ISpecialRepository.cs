namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Enums;
    using MirthSystems.Pulse.Core.Models;

    using NetTopologySuite.Geometries;

    using NodaTime;

    /// <summary>
    /// Repository interface for special promotion entities, extending the base repository with special-specific operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines special-specific data access operations beyond the basic CRUD operations.</para>
    /// <para>It provides methods for retrieving specials with related data, advanced filtering, and determining active status.</para>
    /// <para>Implementations handle the actual data access logic and database interactions.</para>
    /// </remarks>
    public interface ISpecialRepository : IRepository<Special>
    {
        /// <summary>
        /// Gets a special by ID with its associated venue information.
        /// </summary>
        /// <param name="id">The primary key of the special to retrieve.</param>
        /// <returns>The special with its venue information if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method implements eager loading of related entities to reduce database round trips.</para>
        /// <para>Related entities loaded include:</para>
        /// <para>- Venue: The venue offering this special</para>
        /// <para>- Venue.Address: The venue's physical location</para>
        /// </remarks>
        Task<Special?> GetSpecialWithVenueAsync(long id);

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
        /// <para>This method implements server-side pagination with flexible filtering options.</para>
        /// <para>Filtering capabilities include:</para>
        /// <para>- Location-based filtering (specials at venues within a specific distance of a point)</para>
        /// <para>- Text search in special descriptions and venue names</para>
        /// <para>- Type filtering (food, drink, entertainment)</para>
        /// <para>- Expiration filtering to exclude expired specials</para>
        /// <para>Results are ordered by creation date (newest first) and venue name.</para>
        /// </remarks>
        Task<PagedList<Special>> GetPagedSpecialsAsync(
            int page,
            int pageSize,
            Point? location = null,
            double? distanceInMeters = null,
            string? searchTerm = null,
            SpecialTypes? type = null,         
            bool includeExpired = false);

        /// <summary>
        /// Gets all specials for a specific venue.
        /// </summary>
        /// <param name="venueId">The primary key of the venue.</param>
        /// <returns>A list of specials associated with the venue.</returns>
        /// <remarks>
        /// <para>This method retrieves all non-deleted specials for the specified venue.</para>
        /// <para>Results are ordered by start date and then by start time.</para>
        /// <para>This ordering helps display specials in a logical sequence for users.</para>
        /// </remarks>
        Task<List<Special>> GetSpecialsByVenueIdAsync(long venueId);

        /// <summary>
        /// Determines if a special is currently active based on its schedule.
        /// </summary>
        /// <param name="specialId">The primary key of the special.</param>
        /// <param name="referenceTime">The instant in time to check against.</param>
        /// <returns>True if the special is currently active; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method evaluates whether a special is active by checking:</para>
        /// <para>- Start date and time</para>
        /// <para>- End time if specified</para>
        /// <para>- Expiration date if specified</para>
        /// <para>- Recurrence pattern (CRON schedule) if the special is recurring</para>
        /// <para>Special cases handled include:</para>
        /// <para>- End times that cross midnight into the next day</para>
        /// <para>- Recurring specials with complex schedules</para>
        /// </remarks>
        Task<bool> IsSpecialCurrentlyActiveAsync(long specialId, Instant referenceTime);
    }
}
