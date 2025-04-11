namespace Pulse.Core.Contracts
{
    using System.Linq.Expressions;

    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Primary key type</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// Gets entity by its primary key
        /// </summary>
        /// <param name="id">Primary key value</param>
        /// <returns>Entity if found, null otherwise</returns>
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Finds entities based on a predicate
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>Collection of entities matching the criteria</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <param name="userId">ID of the user performing the action</param>
        /// <returns>Added entity</returns>
        Task<TEntity> AddAsync(TEntity entity, string userId);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="userId">ID of the user performing the action</param>
        Task UpdateAsync(TEntity entity, string userId);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Counts entities matching a predicate
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>Count of matching entities</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds multiple entities in a batch operation
        /// </summary>
        /// <param name="entities">Entities to add</param>
        /// <param name="userId">ID of the user performing the action</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities, string userId);

        /// <summary>
        /// Updates multiple entities in a batch operation
        /// </summary>
        /// <param name="entities">Entities to update</param>
        /// <param name="userId">ID of the user performing the action</param>
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, string userId);

        /// <summary>
        /// Removes multiple entities in a batch operation
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
