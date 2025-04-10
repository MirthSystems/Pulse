namespace Pulse.Core.Contracts
{
    using Pulse.Core.Enums;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for Special-specific operations
    /// </summary>
    public interface ISpecialRepository : IRepository<Special, long>
    {
        /// <summary>
        /// Gets all active specials for a specific venue
        /// </summary>
        /// <param name="venueId">Venue ID</param>
        /// <returns>Collection of active specials</returns>
        Task<IEnumerable<Special>> GetActiveForVenueAsync(long venueId);

        /// <summary>
        /// Gets all specials for a specific venue with their tags
        /// </summary>
        /// <param name="venueId">Venue ID</param>
        /// <returns>Collection of specials with their tags</returns>
        Task<IEnumerable<Special>> GetWithTagsForVenueAsync(long venueId);

        /// <summary>
        /// Gets all active specials by type
        /// </summary>
        /// <param name="type">Special type (food, drink, entertainment)</param>
        /// <returns>Collection of active specials of the specified type</returns>
        Task<IEnumerable<Special>> GetActiveByTypeAsync(SpecialTypes type);

        /// <summary>
        /// Gets a special with its tags
        /// </summary>
        /// <param name="id">Special ID</param>
        /// <returns>Special with tags if found, null otherwise</returns>
        Task<Special?> GetWithTagsAsync(long id);

        /// <summary>
        /// Gets a special with its venue
        /// </summary>
        /// <param name="id">Special ID</param>
        /// <returns>Special with venue if found, null otherwise</returns>
        Task<Special?> GetWithVenueAsync(long id);

        /// <summary>
        /// Gets a special with all related data (venue, tags)
        /// </summary>
        /// <param name="id">Special ID</param>
        /// <returns>Special with all related data if found, null otherwise</returns>
        Task<Special?> GetWithAllDataAsync(long id);
    }
}
