namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    using System.Linq.Expressions;

    /// <summary>
    /// Generic repository implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Primary key type</typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IClock _clock;

        public Repository(ApplicationDbContext context, IClock clock)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _clock = clock;
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity, string userId)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.CreatedAt = _clock.GetCurrentInstant();
                entityBase.CreatedByUserId = userId;
            }

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(TEntity entity, string userId)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.UpdatedAt = _clock.GetCurrentInstant();
                entityBase.UpdatedByUserId = userId;
            }

            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
