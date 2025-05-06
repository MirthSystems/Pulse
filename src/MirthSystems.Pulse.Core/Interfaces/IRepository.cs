namespace MirthSystems.Pulse.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations on entities.
    /// </summary>
    /// <typeparam name="T">The entity type this repository manages.</typeparam>
    /// <remarks>
    /// <para>This interface defines a common set of data access operations that all repositories should implement.</para>
    /// <para>It provides a consistent API for basic Create, Read, Update, and Delete operations.</para>
    /// <para>Entity-specific repositories can extend this interface with additional specialized methods.</para>
    /// <para>The repository pattern isolates data access logic from business logic for better separation of concerns.</para>
    /// </remarks>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities of type T.
        /// </summary>
        /// <returns>An IQueryable for further filtering, ordering, or projection.</returns>
        /// <remarks>
        /// <para>This method returns an IQueryable to allow further query composition before execution.</para>
        /// <para>Entity-specific implementations may override this to include default filtering (e.g., excluding deleted items).</para>
        /// <para>Examples of usage:</para>
        /// <para>- Basic retrieval: repository.GetAll().ToListAsync()</para>
        /// <para>- Filtered retrieval: repository.GetAll().Where(x => x.Status == "Active").ToListAsync()</para>
        /// </remarks>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method performs a direct lookup by primary key, which is typically very efficient.</para>
        /// <para>Entity-specific implementations may override this to include related data or apply filters.</para>
        /// <para>Example: await repository.GetByIdAsync(123)</para>
        /// </remarks>
        Task<T?> GetByIdAsync(long id);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The entity with any server-generated properties populated (e.g., auto-generated IDs).</returns>
        /// <remarks>
        /// <para>This method stages the entity for insertion; changes are not persisted until SaveChanges is called.</para>
        /// <para>After this method, the entity is typically being tracked by the context.</para>
        /// <para>Example: await repository.AddAsync(newEntity)</para>
        /// </remarks>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        /// <remarks>
        /// <para>This method stages the entity for update; changes are not persisted until SaveChanges is called.</para>
        /// <para>The entity's state is typically set to Modified after this method.</para>
        /// <para>Example: repository.Update(modifiedEntity)</para>
        /// </remarks>
        T Update(T entity);

        /// <summary>
        /// Deletes an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity to delete.</param>
        /// <returns>True if the entity was found and deleted; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method stages the entity for deletion; changes are not persisted until SaveChanges is called.</para>
        /// <para>Entity-specific implementations may override this to implement soft deletes instead of physical deletes.</para>
        /// <para>Example: await repository.DeleteAsync(123)</para>
        /// </remarks>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// Checks if an entity with the specified primary key exists.
        /// </summary>
        /// <param name="id">The primary key to check.</param>
        /// <returns>True if an entity with the specified primary key exists; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method is more efficient than GetByIdAsync when only checking existence.</para>
        /// <para>Example: await repository.ExistsAsync(123)</para>
        /// </remarks>
        Task<bool> ExistsAsync(long id);
    }
}
