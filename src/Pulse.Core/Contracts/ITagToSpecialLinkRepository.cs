namespace Pulse.Core.Contracts
{
    using System.Linq.Expressions;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for TagToSpecialLink-specific operations
    /// </summary>
    public interface ITagToSpecialLinkRepository
    {
        /// <summary>
        /// Gets a tag-to-special link by its composite primary key
        /// </summary>
        /// <param name="id">Composite key (TagId, SpecialId)</param>
        /// <returns>Link if found, null otherwise</returns>
        Task<TagToSpecialLink?> GetByIdAsync((long TagId, long SpecialId) id);

        /// <summary>
        /// Gets all tag-to-special links
        /// </summary>
        /// <returns>Collection of all links</returns>
        Task<IEnumerable<TagToSpecialLink>> GetAllAsync();

        /// <summary>
        /// Finds links based on a predicate
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>Collection of links matching the criteria</returns>
        Task<IEnumerable<TagToSpecialLink>> FindAsync(Expression<Func<TagToSpecialLink, bool>> predicate);

        /// <summary>
        /// Adds a new link
        /// </summary>
        /// <param name="entity">Link to add</param>
        /// <param name="userId">ID of the user performing the action</param>
        /// <returns>Added link</returns>
        Task<TagToSpecialLink> AddAsync(TagToSpecialLink entity, string userId);

        /// <summary>
        /// Updates an existing link
        /// </summary>
        /// <param name="entity">Link to update</param>
        /// <param name="userId">ID of the user performing the action</param>
        Task UpdateAsync(TagToSpecialLink entity, string userId);

        /// <summary>
        /// Removes a link
        /// </summary>
        /// <param name="entity">Link to remove</param>
        Task DeleteAsync(TagToSpecialLink entity);

        /// <summary>
        /// Gets all tag links for a specific special
        /// </summary>
        /// <param name="specialId">Special ID</param>
        /// <returns>Collection of tag links</returns>
        Task<IEnumerable<TagToSpecialLink>> GetBySpecialIdAsync(long specialId);

        /// <summary>
        /// Gets all special links for a specific tag
        /// </summary>
        /// <param name="tagId">Tag ID</param>
        /// <returns>Collection of special links</returns>
        Task<IEnumerable<TagToSpecialLink>> GetByTagIdAsync(long tagId);

        /// <summary>
        /// Adds a link between a tag and a special
        /// </summary>
        /// <param name="tagId">Tag ID</param>
        /// <param name="specialId">Special ID</param>
        /// <param name="userId">ID of the user performing the action</param>
        /// <returns>Created link</returns>
        Task<TagToSpecialLink> AddLinkAsync(long tagId, long specialId, string userId);

        /// <summary>
        /// Removes a link between a tag and a special
        /// </summary>
        /// <param name="tagId">Tag ID</param>
        /// <param name="specialId">Special ID</param>
        Task RemoveLinkAsync(long tagId, long specialId);
    }
}
