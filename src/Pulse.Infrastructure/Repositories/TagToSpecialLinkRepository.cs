namespace Pulse.Infrastructure.Repositories
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class TagToSpecialLinkRepository : ITagToSpecialLinkRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TagToSpecialLink> _dbSet;
        protected readonly IClock _clock;

        private static readonly Func<ApplicationDbContext, long, Task<List<TagToSpecialLink>>> _getBySpecialIdQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long specialId) =>
                context.TagToSpecialLinks
                    .AsNoTracking()
                    .Include(tsl => tsl.Tag)
                    .Where(tsl => tsl.SpecialId == specialId)
                    .ToList());

        private static readonly Func<ApplicationDbContext, long, Task<List<TagToSpecialLink>>> _getByTagIdQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long tagId) =>
                context.TagToSpecialLinks
                    .AsNoTracking()
                    .Include(tsl => tsl.Special)
                    .Where(tsl => tsl.TagId == tagId)
                    .ToList());

        public TagToSpecialLinkRepository(ApplicationDbContext context, IClock clock)
        {
            _context = context;
            _dbSet = context.TagToSpecialLinks;
            _clock = clock;
        }

        public async Task<TagToSpecialLink?> GetByIdAsync((long TagId, long SpecialId) id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(tsl => tsl.TagId == id.TagId && tsl.SpecialId == id.SpecialId);
        }

        public async Task<IEnumerable<TagToSpecialLink>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TagToSpecialLink>> FindAsync(
            Expression<Func<TagToSpecialLink, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<TagToSpecialLink> AddAsync(TagToSpecialLink entity, string userId)
        {
            entity.CreatedAt = _clock.GetCurrentInstant();
            entity.CreatedByUserId = userId;

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(TagToSpecialLink entity, string userId)
        {
            entity.UpdatedAt = _clock.GetCurrentInstant();
            entity.UpdatedByUserId = userId;

            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(TagToSpecialLink entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TagToSpecialLink>> GetBySpecialIdAsync(long specialId)
        {
            return await _getBySpecialIdQuery(_context, specialId);
        }

        public async Task<IEnumerable<TagToSpecialLink>> GetByTagIdAsync(long tagId)
        {
            return await _getByTagIdQuery(_context, tagId);
        }

        public async Task<TagToSpecialLink> AddLinkAsync(long tagId, long specialId, string userId)
        {
            var link = new TagToSpecialLink
            {
                TagId = tagId,
                SpecialId = specialId,
                CreatedAt = _clock.GetCurrentInstant(),
                CreatedByUserId = userId
            };

            await _dbSet.AddAsync(link);
            return link;
        }

        public async Task RemoveLinkAsync(long tagId, long specialId)
        {
            await _dbSet
                .Where(tsl => tsl.TagId == tagId && tsl.SpecialId == specialId)
                .ExecuteDeleteAsync();
        }
    }
}
