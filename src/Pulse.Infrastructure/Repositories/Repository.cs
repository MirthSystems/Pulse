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

        private static readonly Func<ApplicationDbContext, TKey, Task<TEntity?>> _getByIdQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, TKey id) =>
                context.Set<TEntity>().Find(id));

        public Repository(ApplicationDbContext context, IClock clock)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _clock = clock;
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _getByIdQuery(_context, id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, string userId)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.CreatedAt = _clock.GetCurrentInstant();
                entityBase.CreatedByUserId = userId;
            }

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity, string userId)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.UpdatedAt = _clock.GetCurrentInstant();
                entityBase.UpdatedByUserId = userId;

                _context.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, string userId)
        {
            foreach (var entity in entities)
            {
                if (entity is EntityBase entityBase)
                {
                    entityBase.CreatedAt = _clock.GetCurrentInstant();
                    entityBase.CreatedByUserId = userId;
                }
            }

            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, string userId)
        {
            foreach (var entity in entities)
            {
                if (entity is EntityBase entityBase)
                {
                    entityBase.UpdatedAt = _clock.GetCurrentInstant();
                    entityBase.UpdatedByUserId = userId;
                }
            }

            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
    }
}
