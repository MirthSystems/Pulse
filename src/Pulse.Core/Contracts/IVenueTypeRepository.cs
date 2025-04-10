namespace Pulse.Core.Contracts
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for VenueType-specific operations
    /// </summary>
    public interface IVenueTypeRepository : IRepository<VenueType, int>
    {
        /// <summary>
        /// Gets a venue type by its name
        /// </summary>
        /// <param name="name">Type name</param>
        /// <returns>Venue type if found, null otherwise</returns>
        Task<VenueType?> GetByNameAsync(string name);

        /// <summary>
        /// Gets a venue type with its venues
        /// </summary>
        /// <param name="id">Type ID</param>
        /// <returns>Venue type with venues if found, null otherwise</returns>
        Task<VenueType?> GetWithVenuesAsync(int id);
    }
}
