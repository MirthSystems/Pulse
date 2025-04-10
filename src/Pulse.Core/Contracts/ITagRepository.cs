namespace Pulse.Core.Contracts
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for Tag-specific operations
    /// </summary>
    public interface ITagRepository : IRepository<Tag, long>
    {
        /// <summary>
        /// Gets a tag by its name
        /// </summary>
        /// <param name="name">Tag name without # prefix</param>
        /// <returns>Tag if found, null otherwise</returns>
        Task<Tag?> GetByNameAsync(string name);

        /// <summary>
        /// Gets a tag with its specials
        /// </summary>
        /// <param name="id">Tag ID</param>
        /// <returns>Tag with specials if found, null otherwise</returns>
        Task<Tag?> GetWithSpecialsAsync(long id);

        /// <summary>
        /// Gets the most popular tags by usage count
        /// </summary>
        /// <param name="count">Number of tags to retrieve</param>
        /// <returns>Collection of popular tags</returns>
        Task<IEnumerable<Tag>> GetMostPopularAsync(int count);

        /// <summary>
        /// Increments the usage count for a tag
        /// </summary>
        /// <param name="id">Tag ID</param>
        /// <returns>Updated tag</returns>
        Task<Tag> IncrementUsageCountAsync(long id);
    }
}
