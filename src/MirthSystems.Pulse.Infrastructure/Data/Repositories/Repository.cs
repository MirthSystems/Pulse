namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using MirthSystems.Pulse.Core.Interfaces;

    /// <summary>
    /// Generic repository implementation for basic CRUD operations.
    /// </summary>
    /// <typeparam name="T">The entity type this repository manages.</typeparam>
    /// <remarks>
    /// <para>This class serves as a base implementation for all entity repositories in the system.</para>
    /// <para>It provides standard CRUD operations (Create, Read, Update, Delete) for any entity type.</para>
    /// <para>Derived repositories can override these methods to provide specialized behavior.</para>
    /// <para>The repository pattern abstracts data access logic from business logic for better separation of concerns.</para>
    /// </remarks>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The database context for accessing the database.
        /// </summary>
        /// <remarks>
        /// <para>Protected field accessible to derived repositories.</para>
        /// <para>Provides access to the underlying database context for custom queries.</para>
        /// </remarks>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// The Entity Framework DbSet representing the entity collection.
        /// </summary>
        /// <remarks>
        /// <para>Protected field accessible to derived repositories.</para>
        /// <para>Provides direct access to the entity set for custom LINQ queries.</para>
        /// </remarks>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks>
        /// <para>This constructor initializes the repository with the provided database context.</para>
        /// <para>It also sets up the DbSet for the entity type T.</para>
        /// </remarks>
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Gets all entities of type T.
        /// </summary>
        /// <returns>An IQueryable for further filtering or ordering.</returns>
        /// <remarks>
        /// <para>This method returns an IQueryable to allow further query composition before execution.</para>
        /// <para>This base implementation returns all records; derived classes often override to add filters like soft-delete checks.</para>
        /// <para>Examples of usage:</para>
        /// <para>- Get all venues: _venueRepository.GetAll()</para>
        /// <para>- Get venues with filtering: _venueRepository.GetAll().Where(v => v.Rating > 4)</para>
        /// </remarks>
        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// Gets an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method performs a primary key lookup which is optimized in Entity Framework.</para>
        /// <para>The returned entity is tracked by the context unless tracking is disabled.</para>
        /// <para>Example: await _venueRepository.GetByIdAsync(123)</para>
        /// </remarks>
        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds a new entity to the context.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity with updated information like generated IDs.</returns>
        /// <remarks>
        /// <para>This method only stages the entity for insertion; SaveChanges must be called to persist to the database.</para>
        /// <para>After calling this method, the entity is being tracked by the context.</para>
        /// <para>Example: await _venueRepository.AddAsync(newVenue)</para>
        /// </remarks>
        public virtual async Task<T> AddAsync(T entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            return entry.Entity;
        }

        /// <summary>
        /// Updates an existing entity in the context.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        /// <remarks>
        /// <para>This method only stages the entity for update; SaveChanges must be called to persist to the database.</para>
        /// <para>If the entity is already being tracked, its state is set to Modified.</para>
        /// <para>If the entity is not being tracked, it is attached to the context and marked as Modified.</para>
        /// <para>Example: _venueRepository.Update(existingVenue)</para>
        /// </remarks>
        public virtual T Update(T entity)
        {
            var entry = _dbSet.Update(entity);
            return entry.Entity;
        }

        /// <summary>
        /// Deletes an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key of the entity to delete.</param>
        /// <returns>True if the entity was found and deleted; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method only stages the entity for deletion; SaveChanges must be called to persist to the database.</para>
        /// <para>This is a physical delete operation. For soft delete, override this method in derived classes.</para>
        /// <para>Example: await _venueRepository.DeleteAsync(123)</para>
        /// </remarks>
        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            return true;
        }

        /// <summary>
        /// Determines whether an entity with the specified primary key exists.
        /// </summary>
        /// <param name="id">The primary key value to search for.</param>
        /// <returns>True if an entity with the specified ID exists; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs an efficient existence check without retrieving the entire entity.</para>
        /// <para>Example: await _venueRepository.ExistsAsync(123)</para>
        /// </remarks>
        public virtual async Task<bool> ExistsAsync(long id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
