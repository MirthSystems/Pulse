namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Enums;

    using NetTopologySuite.Geometries;

    using NodaTime;

    public interface ISpecialRepository : IRepository<Special>
    {
        /// <summary>
        /// Gets a special by ID with venue information
        /// </summary>
        Task<Special?> GetSpecialWithVenueAsync(long id);

        /// <summary>
        /// Gets a paged list of specials with optional filtering
        /// </summary>
        Task<(List<Special> specials, int totalCount)> GetPagedSpecialsAsync(
            int page,
            int pageSize,
            Point? location = null,
            double? distanceInMeters = null,
            string? searchTerm = null,
            SpecialTypes? type = null,         
            bool includeExpired = false);

        /// <summary>
        /// Gets all specials for a venue
        /// </summary>
        Task<List<Special>> GetSpecialsByVenueIdAsync(long venueId);

        /// <summary>
        /// Determines if a special is currently active based on its schedule
        /// </summary>
        Task<bool> IsSpecialCurrentlyActiveAsync(long specialId, Instant referenceTime);
    }
}
