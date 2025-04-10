namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class TagRepository : Repository<Tag, long>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<Tag?> GetWithSpecialsAsync(long id)
        {
            return await _dbSet
                .Include(t => t.Specials)
                    .ThenInclude(tsl => tsl.Special)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tag>> GetMostPopularAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(t => t.UsageCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Tag> IncrementUsageCountAsync(long id)
        {
            var tag = await _dbSet.FindAsync(id);

            if (tag == null)
            {
                throw new ArgumentException($"Tag with ID {id} not found", nameof(id));
            }

            tag.UsageCount++;
            return tag;
        }
    }
}
