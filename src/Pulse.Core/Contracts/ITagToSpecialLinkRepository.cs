namespace Pulse.Core.Contracts
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for TagToSpecialLink-specific operations
    /// </summary>
    public interface ITagToSpecialLinkRepository : IRepository<TagToSpecialLink, (long TagId, long SpecialId)>
    {
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
