namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class TagRepository : Repository<Tag, long>, ITagRepository
    {
        private static readonly Func<ApplicationDbContext, string, Task<Tag?>> _getByNameQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, string name) =>
                context.Tags
                    .AsNoTracking()
                    .FirstOrDefault(t => t.Name.ToLower() == name.ToLower()));

        private static readonly Func<ApplicationDbContext, int, Task<List<Tag>>> _getMostPopularQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, int count) =>
                context.Tags
                    .AsNoTracking()
                    .OrderByDescending(t => t.UsageCount)
                    .Take(count)
                    .ToList());

        public TagRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _getByNameQuery(_context, name.ToLower());
        }

        public async Task<Tag?> GetWithSpecialsAsync(long id)
        {
            return await _dbSet
                .AsSplitQuery()
                .AsNoTracking()
                .Include(t => t.Specials)
                    .ThenInclude(tsl => tsl.Special)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tag>> GetMostPopularAsync(int count)
        {
            return await _getMostPopularQuery(_context, count);
        }

        public async Task<Tag> IncrementUsageCountAsync(long id)
        {
            await _context.Tags
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.UsageCount, t => t.UsageCount + 1));

            var tag = await _dbSet.FindAsync(id);

            if (tag == null)
            {
                throw new ArgumentException($"Tag with ID {id} not found", nameof(id));
            }

            return tag;
        }
    }
}
