namespace MirthSystems.Pulse.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(long id);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        T Update(T entity);

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// Checks if an entity with the specified ID exists
        /// </summary>
        Task<bool> ExistsAsync(long id);
    }
}
