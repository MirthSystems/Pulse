namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class TagToSpecialLinkRepository : ITagToSpecialLinkRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TagToSpecialLink> _dbSet;
        protected readonly IClock _clock;

        public TagToSpecialLinkRepository(ApplicationDbContext context, IClock clock)
        {
            _context = context;
            _dbSet = context.TagToSpecialLinks;
            _clock = clock;
        }

        public async Task<TagToSpecialLink?> GetByIdAsync((long TagId, long SpecialId) id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(tsl => tsl.TagId == id.TagId && tsl.SpecialId == id.SpecialId);
        }

        public async Task<IEnumerable<TagToSpecialLink>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TagToSpecialLink>> FindAsync(
            System.Linq.Expressions.Expression<Func<TagToSpecialLink, bool>> predicate)
        {
            return await _dbSet
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
            return await _dbSet
                .Include(tsl => tsl.Tag)
                .Where(tsl => tsl.SpecialId == specialId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TagToSpecialLink>> GetByTagIdAsync(long tagId)
        {
            return await _dbSet
                .Include(tsl => tsl.Special)
                .Where(tsl => tsl.TagId == tagId)
                .ToListAsync();
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
            var link = await _dbSet
                .FirstOrDefaultAsync(tsl => tsl.TagId == tagId && tsl.SpecialId == specialId);

            if (link != null)
            {
                _dbSet.Remove(link);
            }
        }
    }
}
